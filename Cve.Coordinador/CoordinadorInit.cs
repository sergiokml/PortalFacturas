using Cve.Coordinador.Infraestructure;
using Cve.Coordinador.Services.Interfaces;

namespace Cve.Coordinador
{
    public class CoordinadorInit
    {
        #region Miembros
        public IAgentService AgentService { get; }
        public IAuthenticateService AuthenticateService { get; }
        public IAuxiliaryFileService AuxiliaryFileService { get; }
        public IDteService DteService { get; }
        public IInstructionService InstructionService { get; }
        public IParticipantService ParticipantService { get; }
        public IPaymentService PaymentService { get; }
        public IPeriodService PeriodService { get; }
        public IBillingWindowService BillingWindowService { get; }
        public IBillingTypeService BillingTypeService { get; }
        public IGitHubApi IGitHubApi { get; }

        public CoordinadorInit(
            IAgentService agentService,
            IAuthenticateService authenticateService,
            IAuxiliaryFileService auxiliaryFileService,
            IDteService dteService,
            IInstructionService instructionService,
            IParticipantService participantService,
            IPaymentService paymentService,
            IPeriodService periodService,
            IBillingWindowService billingWindowService,
            IBillingTypeService billingTypeService,
            IGitHubApi gitHubApi
        )
        {
            AgentService = agentService;
            AuthenticateService = authenticateService;
            AuxiliaryFileService = auxiliaryFileService;
            DteService = dteService;
            InstructionService = instructionService;
            ParticipantService = participantService;
            PaymentService = paymentService;
            PeriodService = periodService;
            BillingWindowService = billingWindowService;
            BillingTypeService = billingTypeService;
            IGitHubApi = gitHubApi;
        }
        #endregion

        #region Ctor


        #endregion
    }
}
