using Cve.Impuestos.Services.Interfaces;

namespace Cve.Impuestos
{
    public class ImpuestosInit
    {
        #region Miembros
        public ITimbrajeService TimbrajeService { get; }
        public IReTimbrajeService ReTimbrajeService { get; }
        public IContribuyenteService ContribuyenteService { get; }
        public IMisDteService MisDteService { get; }
        public IRegValidaDteService RegValidaDteService { get; }
        public IFirmaDteService FirmaDteService { get; }
        #endregion

        #region Ctor
        public ImpuestosInit(
            ITimbrajeService obtencionFoliosService,
            IContribuyenteService contribuyenteService,
            IReTimbrajeService reTimbrajeService,
            IMisDteService misDteService,
            IRegValidaDteService regValidaDteService,
            IFirmaDteService firmaDteService
        )
        {
            TimbrajeService = obtencionFoliosService;
            ReTimbrajeService = reTimbrajeService;
            ContribuyenteService = contribuyenteService;
            MisDteService = misDteService;
            RegValidaDteService = regValidaDteService;
            FirmaDteService = firmaDteService;
        }
        #endregion
    }
}
