using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.DTO.Team;
using Sneat.MVC.Models.DTO.User;
using Sneat.MVC.Models.Entity;
using Sneat.MVC.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Services
{
    public class TeamService
    {
        private readonly SneatContext _dbContext;
        public TeamService(SneatContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Team Management

        public async Task<IPagedList<TeamOutputModel>> SearchTeam(int page, int limit, string search = "")
        {
            try
            {
                var list = await GetListTeam(search);
                var listPaging = list.ToPagedList(page, limit);

                return listPaging;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<TeamOutputModel>().ToPagedList(1 , 1);
            }
        }

        public async Task<List<TeamOutputModel>> GetListTeam(string search = "")
        {
            try
            {
                search = Utils.RemoveDiacritics(search);
                var listTeams = _dbContext.Teams
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .Select(x => new
                    {
                        x.ID,
                        x.Name, 
                        x.Description,
                        x.Status,
                        TechStack = x.TechStack,
                        x.CreatedDate,
                        x.UpdatedDate,
                        UserTeams = x.UserTeams
                        .Select(ur => new UserTeamOutputModel
                        {
                            UserID = ur.User.ID,
                            UserName = ur.User.UserName,
                            UserAvatar = !string.IsNullOrEmpty(ur.User.Avatar) ? ur.User.Avatar : SystemParam.DEFAULT_SYSTEM_IMAGE,
                        })
                    })
                    .AsEnumerable()
                    .Select(x => new TeamOutputModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description,
                        Status = (int)x.Status,
                        TechStack = Utils.ConvertStringToListInteger(x.TechStack),
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate,
                        UserTeams = x.UserTeams.ToList()
                    })
                    .Where(x => string.IsNullOrEmpty(search)
                        || Utils.RemoveDiacritics(x.Name).Contains(search))
                    .OrderByDescending(x => x.ID)
                    .ToList();

                return listTeams;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<TeamOutputModel>();
            }
        }

        public async Task<int> CreateTeam(TeamInputModel input)
        {
            try
            {
                var existedTeam = await _dbContext.Teams
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.Name.ToLower() == input.Name.ToLower())
                    .FirstOrDefaultAsync();
                if (existedTeam != null)
                    return SystemParam.EXISTED_TEAM_NAME_ERR;

                var newTeam = new Team
                {
                    Name = input.Name,
                    TechStack = Utils.ConvertListIntegerToString(input.TechStack),
                    Description = input.Description,
                    Status = Status.ACTIVE,
                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                _dbContext.Teams.Add(newTeam);

                if (input.UserIds.Count > 0)
                {
                    foreach (var userId in input.UserIds)
                    {
                        var userTeam = new UserTeam
                        {
                            UserID = userId,
                            TeamID = newTeam.ID
                        };
                        _dbContext.UserTeams.Add(userTeam);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<int> UpdateTeam(TeamOutputModel input)
        {
            try
            {
                var existedTeam = await _dbContext.Teams
                  .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                      && x.Name.ToLower() == input.Name.ToLower()
                      && x.ID != input.ID)
                  .FirstOrDefaultAsync();
                if (existedTeam != null)
                    return SystemParam.EXISTED_TEAM_NAME_ERR;

                var team = await _dbContext.Teams
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                            && x.ID == input.ID)
                    .FirstOrDefaultAsync();
                if (team == null)
                    return SystemParam.TEAM_NOT_FOUND_ERR;

                team.Name = input.Name;
                team.TechStack = Utils.ConvertListIntegerToString(input.TechStack);
                team.Description = input.Description;
                team.UpdatedDate = DateTime.Now;
                team.Status = (Status)input.Status;
                _dbContext.UserTeams.RemoveRange(team.UserTeams);
                if (input.UserIds.Count > 0)
                {
                    foreach (var userId in input.UserIds)
                    {
                        var userTeam = new UserTeam
                        {
                            UserID = userId,
                            TeamID = team.ID
                        };
                        _dbContext.UserTeams.Add(userTeam);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<TeamOutputModel> DetailTeam(int ID)
        {
            try
            {
                var team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.IsDeleted == SystemParam.IS_NOT_DELETED && x.ID == ID);
                var teamDetail = new TeamOutputModel
                {
                    ID = ID,
                    Name = team.Name,
                    TechStack = Utils.ConvertStringToListInteger(team.TechStack),
                    Description = team.Description,
                    Status = (int)team.Status,
                    CreatedDate = team.CreatedDate,
                    UpdatedDate = team.UpdatedDate,
                    UserIds = team.UserTeams.Select(x => x.UserID).ToList(),
                };

                return teamDetail;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new TeamOutputModel();
            }
        }

        public async Task<int> RemoveTeam(int ID)
        {
            try
            {
                var team = await _dbContext.Teams
                 .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                         && x.ID == ID)
                 .FirstOrDefaultAsync();
                if (team == null)
                    return SystemParam.TEAM_NOT_FOUND_ERR;

                team.IsDeleted = SystemParam.IS_DELETED;
                _dbContext.UserTeams.RemoveRange(team.UserTeams);

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }
        #endregion

        #region Project Management
        
        public async Task<List<UpdateUserInputModel>> ListUserByTeam(List<int> teamIds)
        {
            try
            {
                var userIds = await _dbContext.UserTeams
                    .Where(x => x.User.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.User.Status == Status.ACTIVE
                        && teamIds.Contains(x.TeamID))
                    .Select(x => x.UserID)
                    .ToListAsync();

                var users = await _dbContext.Users
                    .Where(x => userIds.Contains(x.ID))
                    .Select(x => new UpdateUserInputModel
                    {
                        ID = x.ID,
                        Name = x.UserName,
                        Avatar = x.Avatar,
                    })
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<UpdateUserInputModel>();
            }
        }

        #endregion
    }
}