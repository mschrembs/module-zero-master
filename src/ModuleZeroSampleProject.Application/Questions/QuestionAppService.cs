// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuestionAppService.cs" company="">
//   
// </copyright>
// <summary>
//   The question app service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ModuleZeroSampleProject.Questions
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;

    using Abp.Application.Services;
    using Abp.Application.Services.Dto;
    using Abp.Authorization;
    using Abp.AutoMapper;
    using Abp.Configuration;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Linq.Extensions;
    using Abp.Runtime.Session;
    using Abp.UI;

    using ModuleZeroSampleProject.Configuration;
    using ModuleZeroSampleProject.Questions.Dto;
    using ModuleZeroSampleProject.Users;

    /// <summary>
    ///     The question app service.
    /// </summary>
    [AbpAuthorize]
    public class QuestionAppService : ApplicationService, IQuestionAppService
    {
        #region Fields

        /// <summary>
        ///     The _answer repository.
        /// </summary>
        private readonly IRepository<Answer> answerRepository;

        /// <summary>
        ///     The _question domain service.
        /// </summary>
        private readonly QuestionDomainService questionDomainService;

        /// <summary>
        ///     The _question repository.
        /// </summary>
        private readonly IRepository<Question> questionRepository;

        /// <summary>
        ///     The _unit of work manager.
        /// </summary>
        private readonly IUnitOfWorkManager unitOfWorkManager;

        /// <summary>
        ///     The _user repository.
        /// </summary>
        private readonly IRepository<User, long> userRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionAppService"/> class.
        /// </summary>
        /// <param name="questionRepository">
        /// The question repository.
        /// </param>
        /// <param name="answerRepository">
        /// The answer repository.
        /// </param>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        /// <param name="questionDomainService">
        /// The question domain service.
        /// </param>
        /// <param name="unitOfWorkManager">
        /// The unit of work manager.
        /// </param>
        public QuestionAppService(
            IRepository<Question> questionRepository, 
            IRepository<Answer> answerRepository, 
            IRepository<User, long> userRepository, 
            QuestionDomainService questionDomainService, 
            IUnitOfWorkManager unitOfWorkManager)
        {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
            this.userRepository = userRepository;
            this.questionDomainService = questionDomainService;
            this.unitOfWorkManager = unitOfWorkManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The accept answer.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        public void AcceptAnswer(EntityRequestInput input)
        {
            Answer answer = this.answerRepository.Get(input.Id);
            this.questionDomainService.AcceptAnswer(answer);
        }

        /// <summary>
        /// The create question.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [AbpAuthorize("CanCreateQuestions")] // An example of permission checking
        public async Task CreateQuestion(CreateQuestionInput input)
        {
            await this.questionRepository.InsertAsync(new Question(input.Title, input.Text));
        }

        /// <summary>
        /// The get question.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="GetQuestionOutput"/>.
        /// </returns>
        /// <exception cref="UserFriendlyException">
        /// </exception>
        public GetQuestionOutput GetQuestion(GetQuestionInput input)
        {
            Question question =
                this.questionRepository.GetAll()
                    .Include(q => q.CreatorUser)
                    .Include(q => q.Answers)
                    .Include("Answers.CreatorUser")
                    .FirstOrDefault(q => q.Id == input.Id);

            if (question == null)
            {
                throw new UserFriendlyException("There is no such a question. Maybe it's deleted.");
            }

            if (input.IncrementViewCount)
            {
                question.ViewCount++;
            }

            return new GetQuestionOutput { Question = question.MapTo<QuestionWithAnswersDto>() };
        }

        /// <summary>
        /// The get questions.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="PagedResultOutput"/>.
        /// </returns>
        public PagedResultOutput<QuestionDto> GetQuestions(GetQuestionsInput input)
        {
            if (input.MaxResultCount <= 0)
            {
                input.MaxResultCount =
                    this.SettingManager.GetSettingValue<int>(MySettingProvider.QuestionsDefaultPageSize);
            }

            int questionCount = this.questionRepository.Count();
            List<Question> questions =
                this.questionRepository.GetAll()
                    .Include(q => q.CreatorUser)
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToList();

            return new PagedResultOutput<QuestionDto>
                       {
                           TotalCount = questionCount, 
                           Items = questions.MapTo<List<QuestionDto>>()
                       };
        }

        /// <summary>
        /// The submit answer.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="SubmitAnswerOutput"/>.
        /// </returns>
        [AbpAuthorize("CanAnswerToQuestions")]
        public SubmitAnswerOutput SubmitAnswer(SubmitAnswerInput input)
        {
            Question question = this.questionRepository.Get(input.QuestionId);
            User currentUser = this.userRepository.Get(this.CurrentSession.GetUserId());

            question.AnswerCount++;

            Answer answer =
                this.answerRepository.Insert(new Answer(input.Text) { Question = question, CreatorUser = currentUser });

            this.unitOfWorkManager.Current.SaveChanges();

            return new SubmitAnswerOutput { Answer = answer.MapTo<AnswerDto>() };
        }

        /// <summary>
        /// The vote down.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="VoteChangeOutput"/>.
        /// </returns>
        public VoteChangeOutput VoteDown(EntityRequestInput input)
        {
            Question question = this.questionRepository.Get(input.Id);
            question.VoteCount--;
            return new VoteChangeOutput(question.VoteCount);
        }

        /// <summary>
        /// The vote up.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="VoteChangeOutput"/>.
        /// </returns>
        public VoteChangeOutput VoteUp(EntityRequestInput input)
        {
            Question question = this.questionRepository.Get(input.Id);
            question.VoteCount++;
            return new VoteChangeOutput(question.VoteCount);
        }

        #endregion
    }
}