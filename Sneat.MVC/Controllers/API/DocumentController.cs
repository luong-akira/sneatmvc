using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Document;
using Sneat.MVC.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sneat.MVC.Controllers.API
{
    public class DocumentController : ApiController
    {
        protected SneatContext Context;
        public DocumentService _documentService;

        public SneatContext GetContext()
        {
            if (Context == null)
            {
                Context = new SneatContext();
            }
            return Context;
        }

        public DocumentController()
        {
            _documentService = new DocumentService(this.GetContext());
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListDocumentPaging(
            string search = "", int? projectID = null, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.MAX_ROW_IN_LIST_WEB)
        {
            return await _documentService.GetListDocumentPaging(search, projectID, page, limit);
        }

        [HttpPost]
        public async Task<JsonResultModel> CreateDocument(DocumentInputModel input)
        {
            return await _documentService.CreateDocument(input);
        }

        [HttpPost]
        public async Task<JsonResultModel> UpdateDocument(DocumentOutputModel input)
        {
            return await _documentService.UpdateDocument(input);
        }

        [HttpGet]
        public async Task<JsonResultModel> DetailDocument(int ID)
        {
            return await _documentService.DetailDocument(ID);
        }

        [HttpPost]
        public async Task<JsonResultModel> DeleteDocument(int ID)
        {
            return await _documentService.DeleteDocument(ID);
        }
    }
}