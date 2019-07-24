using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartApartmentDataTestDataModule.DTOs;
using SmartApartmentDataTestDataModule.Entity;
using SmartApartmentDataTestDataModule.Exceptions;
using SmartApartmentDataTestDataModule.Models;
using SmartApartmentDataTestUtility.Encryption.AES;

namespace SmartApartmentDataTestDataModule.Data_Access
{
    public class SmartApartmentDataTestDataAccess : ISmartApartmentDataTestDataAccess
    {
        #region Private Fields

        private const string EncryptionKey = "8M5%'f+lta#..;JK}pX0J7^%A4bh${l'";
        private readonly Entity.SmartApartmentEntities _entities;

        #endregion

        #region Constructor

        public SmartApartmentDataTestDataAccess()
        {
            var encryptedConnectionString = ConfigurationManager.ConnectionStrings["SmartApartmentEntities"];
            var connectionString = AESEncryption.Decrypt(encryptedConnectionString.ConnectionString, EncryptionKey);
            _entities = new SmartApartmentEntities(connectionString);
        }


        #endregion

        #region Classes

        public ClassModel GetClassByName(string className)
        {
            var classModel = ClassModel.FromDbObject(_entities.classes.FirstOrDefault(x => x.name.Equals(className)));

            if (classModel == null)
                throw ClassNotFoundException.FromName(className);

            return classModel;
        }

        public ClassModel GetClassByID(int id)
        {
            var classModel = ClassModel.FromDbObject(_entities.classes.FirstOrDefault(x => x.pk_id == id));

            if (classModel == null)
                throw ClassNotFoundException.FromId(id);

            return classModel;
        }

        public List<string> GetClassNames()
        {
            return _entities.classes.Select(x => x.name).ToList();
        }

        public void AddClass(ClassModelDto dto)
        {
            _entities.classes.Add(dto.ToDbObject());
            _entities.SaveChanges();
        }

        public void DeleteClassByName(string className)
        {
            var classToRemove = _entities.classes.FirstOrDefault(x => x.name.Equals(className));

            if (classToRemove == null)
                throw ClassNotFoundException.FromName(className);

            _entities.classes.Remove(classToRemove);
            _entities.SaveChanges();
        }

        public void DeleteClassByID(int id)
        {
            var classToRemove = _entities.classes.FirstOrDefault(x => x.pk_id == id);

            if (classToRemove == null)
                throw ClassNotFoundException.FromId(id);

            _entities.classes.Remove(classToRemove);
            _entities.SaveChanges();
        }

        #endregion

        #region Factions

        public FactionModel GetFactionByName(string factionName)
        {
            return FactionModel.FromDbObject(_entities.factions.FirstOrDefault(x => x.name.Equals(factionName)));
        }

        public FactionModel GetFactionByID(int id)
        {
            return FactionModel.FromDbObject(_entities.factions.FirstOrDefault(x => x.pk_id == id));
        }

        public List<string> GetFactionNames()
        {
            return _entities.factions.Select(x => x.name).ToList();
        }

        public void AddFaction(FactionModelDto dto)
        {
            _entities.factions.Add(dto.ToDbObject());
            _entities.SaveChanges();
        }

        public void DeleteFactionByName(string factionName)
        {
            var factionToRemove = _entities.factions.FirstOrDefault(x => x.name.Equals(factionName));

            if (factionToRemove == null)
                throw FactionNotFoundException.FromName(factionName);

            _entities.factions.Remove(factionToRemove);
            _entities.SaveChanges();
        }

        public void DeleteFactionByID(int id)
        {
            var factionToRemove = _entities.factions.FirstOrDefault(x => x.pk_id == id);

            if (factionToRemove == null)
                throw FactionNotFoundException.FromId(id);

            _entities.factions.Remove(factionToRemove);
            _entities.SaveChanges();
        }

        #endregion

        #region Races

        public RaceModel GetRaceByName(string raceName)
        {
            return RaceModel.FromDbObject(_entities.races.FirstOrDefault(x => x.name.Equals(raceName)));
        }

        public RaceModel GetRaceByID(int id)
        {
            return RaceModel.FromDbObject(_entities.races.FirstOrDefault(x => x.pk_id == id));
        }

        public List<string> GetRaceNames()
        {
            return _entities.races.Select(x => x.name).ToList();
        }

        public void AddRace(RaceModelDto dto)
        {
            _entities.races.Add(dto.ToDbObject());
            _entities.SaveChanges();
        }

        public void DeleteRaceByName(string raceName)
        {
            var raceToRemove = _entities.races.FirstOrDefault(x => x.name.Equals(raceName));

            if (raceToRemove == null)
                throw RaceNotFoundException.FromName(raceName);

            _entities.races.Remove(raceToRemove);
            _entities.SaveChanges();
        }

        public void DeleteRaceByID(int id)
        {
            var raceToRemove = _entities.races.FirstOrDefault(x => x.pk_id == id);

            if (raceToRemove == null)
                throw RaceNotFoundException.FromId(id);

            _entities.races.Remove(raceToRemove);
            _entities.SaveChanges();
        }


        #endregion

        #region User

        public void RegisterUser(string auth0Subject)
        {
            _entities.users.Add(new Entity.user
            {
                auth0_subject = auth0Subject
            });
            _entities.SaveChanges();
        }

        public UserModel GetUserByAuth0Subject(string auth0Subject)
        {
            return UserModel.FromDbObject(_entities.users.FirstOrDefault(x => x.auth0_subject.Equals(auth0Subject)));
        }

        #endregion

        #region Player

        public void AddPlayer(PlayerModelDto dto, int userId)
        {
            if(_entities.classes.FirstOrDefault(x => x.pk_id == dto.ClassId) == null)
                throw ClassNotFoundException.FromId(dto.ClassId);
            if (_entities.factions.FirstOrDefault(x => x.pk_id == dto.FactionId) == null)
                throw FactionNotFoundException.FromId(dto.FactionId);
            if (_entities.races.FirstOrDefault(x => x.pk_id == dto.RaceId) == null)
                throw RaceNotFoundException.FromId(dto.RaceId);
            if (_entities.players.FirstOrDefault(x => x.name.Equals(dto.Name)) != null)
                throw new PlayerAlreadyExistsException();

            _entities.players.Add(dto.ToDbObject(userId));
            _entities.SaveChanges();
        }

        public PlayerModel GetPlayerByName(string playerName)
        {
            var playerModel =
                PlayerModel.FromDbObject(_entities.players.FirstOrDefault(x => x.name.Equals(playerName)));

            if (playerModel == null)
                throw PlayerNotFoundException.FromName(playerName);

            return playerModel;
        }

        public PlayerModel GetPlayerByUserId(int userId)
        {
            var playerModel = PlayerModel.FromDbObject(_entities.players.FirstOrDefault(x => x.fk_user == userId));

            if (playerModel == null)
                throw PlayerNotFoundException.FromUserId(userId);

            return playerModel;
        }

        #endregion

        #region Guild

        public void AddGuild(GuildModelDto dto)
        {
            if(_entities.guilds.FirstOrDefault(x => x.name.Equals(dto.Name)) != null)
                throw new GuildAlreadyExistsException();
            if (_entities.factions.FirstOrDefault(x => x.pk_id == dto.FactionId) == null)
                throw FactionNotFoundException.FromId(dto.FactionId);

            _entities.guilds.Add(dto.ToDbObject());
            _entities.SaveChanges();
        }

        public void AddGuildMember(string playerName, string guildName)
        {
            var getPlayerModel = PlayerModel.FromDbObject(_entities.players.FirstOrDefault(x => x.name.Equals(playerName)));

            if (getPlayerModel == null)
                throw PlayerNotFoundException.FromName(playerName);

            var guildModel = GuildModel.FromDbObject(_entities.guilds.FirstOrDefault(x => x.name.Equals(guildName)));

            if (guildModel == null)
                throw GuildNotFoundException.FromName(guildName);

            _entities.guild_members.Add(new guild_members
            {
                fk_guild = guildModel.Id,
                fk_player = getPlayerModel.Id
            });
            _entities.SaveChanges();
        }

        public List<PlayerModel> GetGuildMembers(string guildName)
        {
            var getGuildModel = GuildModel.FromDbObject(_entities.guilds.FirstOrDefault(x => x.name.Equals(guildName)));

            if (getGuildModel == null)
                throw GuildNotFoundException.FromName(guildName);

            return _entities.guild_members.Include("player").Where(x => x.fk_guild == getGuildModel.Id).Select(x => x.player).ToList().Select(PlayerModel.FromDbObject).ToList();
        }

        #endregion
    }
}
