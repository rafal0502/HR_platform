using CommunityCertForT;
using CommunityCertForT.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication.Helpers
{
    public class UserHelper : IUserHelper
    {

        private IConfiguration _configuration;
        private AppSettings AppSettings { get; set; }
        public UserHelper(IConfiguration Configuration)
        {
            _configuration = Configuration;
            AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
        }
        public bool isAdmin(IEnumerable<Claim> userClaims)
        {
            AADGraph graph = new AADGraph(AppSettings);
            string groupName = "Admins";
            string groupId = AppSettings.AADGroups.FirstOrDefault(g =>
            String.Compare(g.Name, groupName) == 0).Id;
            return graph.IsUserInGroup(userClaims, groupId).Result;
        }
        public bool isHRTeam(IEnumerable<Claim> userClaims)
        {
            AADGraph graph = new AADGraph(AppSettings);
            string groupName = "HRTeam";
            string groupId = AppSettings.AADGroups.FirstOrDefault(g =>
            String.Compare(g.Name, groupName) == 0).Id;
            return graph.IsUserInGroup(userClaims, groupId).Result;
        }
    }
    public interface IUserHelper
    {
        bool isAdmin(IEnumerable<Claim> userClaims);
        bool isHRTeam(IEnumerable<Claim> userClaims);
    }

}

