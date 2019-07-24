using SmartApartmentDataTestDataModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartApartmentDataTestDataModule.DTOs;

namespace SmartApartmentDataTestDataModule.Data_Access
{
    public interface ISmartApartmentDataTestDataAccess
    {
        #region Class
        ClassModel GetClassByName(string className);
        ClassModel GetClassByID(int id);
        List<string> GetClassNames();
        void AddClass(ClassModelDto dto);
        void DeleteClassByName(string className);
        void DeleteClassByID(int id);
        #endregion

        #region Faction
        FactionModel GetFactionByName(string className);
        FactionModel GetFactionByID(int id);
        List<string> GetFactionNames();
        void AddFaction(FactionModelDto dto);
        void DeleteFactionByName(string factionName);
        void DeleteFactionByID(int id);
        #endregion

        #region Race
        RaceModel GetRaceByName(string raceName);
        RaceModel GetRaceByID(int id);
        List<string> GetRaceNames();
        void AddRace(RaceModelDto dto);
        void DeleteRaceByName(string raceName);
        void DeleteRaceByID(int id);
        #endregion

        #region User

        void RegisterUser(string auth0Subject);
        UserModel GetUserByAuth0Subject(string auth0Subject);

        #endregion

        #region Player

        void AddPlayer(PlayerModelDto dto, int userId);
        PlayerModel GetPlayerByName(string playerName);
        PlayerModel GetPlayerByUserId(int userId);

        #endregion

        #region Guild

        void AddGuild(GuildModelDto dto);
        void AddGuildMember(string playerName, string guildName);
        List<PlayerModel> GetGuildMembers(string guildName);

        #endregion
    }
}
