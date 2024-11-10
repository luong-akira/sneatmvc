using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Document;
using Sneat.MVC.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Services
{
    public class DocumentService
    {
        private readonly SneatContext _dbContext;
        public ResponseService _responseService = new ResponseService();
        public DocumentService(SneatContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResultModel> GetListDocumentPaging(
            string search = "", int? projectID = null, int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.MAX_ROW_IN_LIST_WEB)
        {
            try
            {
                var list = await GetListDocument(search, projectID);
                var listPaging = list.ToPagedList(page, limit);
                var paging = new PagingModel
                {
                    Page = page,
                    Limit = limit,
                    TotalItemCount = listPaging.TotalItemCount,
                };

                return _responseService.SuccessPaging(SystemParam.MESSAGE_SUCCESS, listPaging, paging);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

        public async Task<List<DocumentOutputModel>> GetListDocument(string search = "", int? projectID = null)
        {
            try
            {
                search = Utils.RemoveDiacritics(search);
                var listDocuments = _dbContext.Documents
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && (!string.IsNullOrEmpty(search) ?
                         x.Title.Contains(search) : true)
                        && (projectID.HasValue ? x.ProjectID == projectID.Value : true)
                        );

                var listResult = listDocuments
                        .Select(x => new DocumentOutputModel
                        {
                            ID = x.ID,
                            Title = x.Title,
                            Content = x.Content,
                            DocumentAttachment = x.DocumentAttachment,
                            ProjectID = x.ProjectID,
                            ProjectName = x.Project != null ? x.Project.Name : string.Empty,
                            CreateDate = x.CreatedDate
                        })
                        .OrderByDescending(x => x.ID)
                        .ToList();

                return listResult;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<DocumentOutputModel>();
            }
        }

        public async Task<JsonResultModel> CreateDocument(DocumentInputModel input)
        {
            try
            {
                var existedDocument = _dbContext.Documents
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                         && x.Title.ToLower() == input.Title.ToLower())
                    .Count();
                if (existedDocument > 0)
                    input.Title = $"{input.Title}({existedDocument + 1})";

                var newDoucument = new Document
                {
                    Title = input.Title,
                    Content = input.Content,
                    DocumentAttachment = input.DocumentAttachment,
                    ProjectID = input.ProjectID,

                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    CreatedDate = DateTime.Now,
                };
                _dbContext.Documents.Add(newDoucument);

                await _dbContext.SaveChangesAsync();
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, newDoucument);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

        public async Task<JsonResultModel> UpdateDocument(DocumentOutputModel input)
        {
            try
            {
                var existedDocument = _dbContext.Documents
                  .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                       && x.Title.ToLower() == input.Title.ToLower()
                       && x.ID != input.ID)
                  .Count();
                if (existedDocument > 0)
                    input.Title = $"{input.Title}({existedDocument + 1})";

                var document = await _dbContext.Documents
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                            && x.ID == input.ID)
                    .FirstOrDefaultAsync();
                if (document == null)
                    return _responseService.ErrorResult(SystemParam.DOCUMENT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                document.Title = input.Title;
                document.Content = input.Content;
                document.DocumentAttachment = input.DocumentAttachment;
                document.UpdatedDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, null);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

        public async Task<JsonResultModel> DetailDocument(int ID)
        {
            try
            {
                var document = await _dbContext.Documents
                    .FirstOrDefaultAsync(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == ID);
                if (document == null)
                    return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, new DocumentOutputModel());

                var documentDetail = new DocumentOutputModel
                {
                    ID = ID,
                    Title = document.Title,
                    Content = document.Content,
                    DocumentAttachment = document.DocumentAttachment,
                    ProjectID = document.ProjectID,
                };

                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, documentDetail);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

        public async Task<JsonResultModel> DeleteDocument(int ID)
        {
            try
            {
                var document = await _dbContext.Documents
                   .FirstOrDefaultAsync(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                       && x.ID == ID);
                if (document == null)
                    return _responseService.ErrorResult(SystemParam.DOCUMENT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                document.IsDeleted = SystemParam.IS_DELETED;

                await _dbContext.SaveChangesAsync();
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, ID);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

    }
}