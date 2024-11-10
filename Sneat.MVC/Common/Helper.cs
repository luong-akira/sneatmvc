using Sneat.MVC.Models.DTO.Team;
using Sneat.MVC.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Common
{
    public class Helper
    {
        #region Convert tech stack enum
        private static readonly Dictionary<TechStack, string> TechStackNames = new Dictionary<TechStack, string>
        {
            { TechStack.DOTNET, ".NET" },
            { TechStack.CSHARP, "C#" },
            { TechStack.NODEJS, "Node.js" },
            { TechStack.REACTJS, "React.js" },
            { TechStack.REACTNATIVE, "React Native" },
            { TechStack.TESTER, "Tester" },
            { TechStack.BA, "BA" },
            { TechStack.DESIGNER, "Designer" },
            { TechStack.SERVER, "Server" }
        };

        public static List<TechStackModel> GetTechStackModels()
        {
            return Enum.GetValues(typeof(TechStack))
                       .Cast<TechStack>()
                       .Select(ts => new TechStackModel
                       {
                           Type = (int)ts,
                           Name = TechStackNames[ts]
                       })
                       .ToList();
        }
        #endregion
    }
}