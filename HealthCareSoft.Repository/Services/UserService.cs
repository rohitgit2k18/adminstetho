using AutoMapper;
using HealthCareSoft.Entity;
using HealthCareSoft.Entity.UnitOfWork;
using HealthCareSoft.Repository.Interfaces;
using HealthCareSoft.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace HealthCareSoft.Repository.Services
{
    public class UserService : IUserServices
    {
        private readonly UnitOfWork _unitOfWork;
        public UserService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public UserEntity GetUserById(int userId)
        {

            var user = _unitOfWork.UserRepository.GetByID(userId);
            user.Password = CryptorEngine.Decrypt(user.Password, true);           
            if (user != null)
            {
                Mapper.CreateMap<UserProfile, UserEntity>();
                var productModel = Mapper.Map<UserProfile, UserEntity>(user);
                return productModel;
            }
            return null;
        }
        public UserEntity GetUserByName(string userId,string password)
        {
            //MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            //byte[] bs = Encoding.UTF8.GetBytes(password);
            //bs = x.ComputeHash(bs);
            //StringBuilder pwd = new StringBuilder();
            //foreach (byte b in bs)
            //{
            //    pwd.Append(b.ToString("x2").ToUpperInvariant());
            //}
            string pwd = CryptorEngine.Encrypt(password, true);
            var user = _unitOfWork.UserRepository.Get(u => u.Email == userId && u.Password == pwd.ToString());
            if (user != null)
            {
                Mapper.CreateMap<UserProfile, UserEntity>();
                var productModel = Mapper.Map<UserProfile, UserEntity>(user);
                return productModel;
            }
            return null;
        }
        public bool GetUserByUserName(string uname)
        {
            bool result = true;
            var user = _unitOfWork.UserRepository.Get(u => u.FirstName == uname);
            if (user != null)
            result = false;
           return result;
        }
        public bool GetUserByEmail(string email)
        {
            bool result = true;
            var user = _unitOfWork.UserRepository.Get(u => u.Email == email);
            if (user != null)
                result = false;
            return result;
        }
        public bool DeleteUser(int userId)
        {
            var success = false;
            if (userId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {

                        _unitOfWork.UserRepository.Delete(user);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
        public bool UserStatus(int userId)
        {
            var success = false;
            if (userId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {
                        user.Id = userId;
                        if (user.IsActive == false)
                        {
                            user.IsActive = true;
                        }
                        else
                        {
                            user.IsActive = false;
                        }
                        try
                        {
                            _unitOfWork.UserRepository.Update(user);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                }
            }
            return success;
        }
        public IEnumerable<UserEntity> GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll().ToList();

            //foreach(var item in users)
            //{
            //    item.Password=  CryptorEngine.Decrypt(item.Password, true);
            //}
            if (users.Any())
            {     
                Mapper.CreateMap<UserProfile, UserEntity>();
                //Mapper.CreateMap<User, UserEntity>().ForMember(user => user.UserStatus,
                //            map => map.MapFrom(entity => entity.Status));
                var usersModel = Mapper.Map<List<UserProfile>, List<UserEntity>>(users);
               
                return usersModel;
            }
            return null;
        }
        public IEnumerable<UserEntity> GetAllSubAdmin()
        {
            var users = _unitOfWork.UserRepository.GetManyQueryable(m => m.RoleId == 4).ToList();
            //foreach (var item in users)
            //{
            //    item.Password = CryptorEngine.Decrypt(item.Password, true);
            //}
            if (users.Any())
            {
                Mapper.CreateMap<UserProfile, UserEntity>();
                //Mapper.CreateMap<User, UserEntity>().ForMember(user => user.UserStatus,
                //            map => map.MapFrom(entity => entity.Status));
                var usersModel = Mapper.Map<List<UserProfile>, List<UserEntity>>(users);
                return usersModel;
            }
            return null;
        }
        public IEnumerable<UserEntity> GetAllDoctors()
        {
            var users = _unitOfWork.UserRepository.GetManyQueryable(m => m.RoleId ==1).ToList();
            //foreach (var item in users)
            //{
            //    item.Password = CryptorEngine.Decrypt(item.Password, true);
            //}
            if (users.Any())
            {
                Mapper.CreateMap<UserProfile, UserEntity>();
                //Mapper.CreateMap<User, UserEntity>().ForMember(user => user.UserStatus,
                //            map => map.MapFrom(entity => entity.Status));
                var usersModel = Mapper.Map<List<UserProfile>, List<UserEntity>>(users);
                return usersModel;
            }
            return null;
        }
        public IEnumerable<UserEntity> GetAllPatients()
        {
            var users = _unitOfWork.UserRepository.GetManyQueryable(m => m.RoleId == 3).ToList();
            //foreach (var item in users)
            //{
            //    item.Password = CryptorEngine.Decrypt(item.Password, true);
            //}
            if (users.Any())
            {
                Mapper.CreateMap<UserProfile, UserEntity>();
                //Mapper.CreateMap<User, UserEntity>().ForMember(user => user.UserStatus,
                //            map => map.MapFrom(entity => entity.Status));
                var usersModel = Mapper.Map<List<UserProfile>, List<UserEntity>>(users);
                return usersModel;
            }
            return null;
        }
        public long CreateUser(UserEntity userEntity)
        {
            //MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            //byte[] bs = Encoding.UTF8.GetBytes(userEntity.Password);
            //bs = x.ComputeHash(bs);
            //StringBuilder pwd = new StringBuilder();
            //foreach (byte b in bs)
            //{
            //    pwd.Append(b.ToString("x2").ToUpperInvariant());
            //}
            string pwd = CryptorEngine.Encrypt(userEntity.Password, true);
            using (var scope = new TransactionScope())
            {
                var user = new UserProfile
                {
                    FirstName = userEntity.FirstName,
                    Email= userEntity.Email,
                    Password= pwd.ToString(),
                    MiddelName = userEntity.MiddelName,
                    LastName = userEntity.LastName,
                    Address = userEntity.Address,
                    PostalCode = userEntity.PostalCode,
                    PhoneNo = userEntity.PhoneNo,
                    Gender = userEntity.Gender,
                    ProfilePicture = userEntity.ProfilePicture,
                    IsActive = userEntity.IsActive,
                    RoleId = userEntity.RoleId,
                    CreatedBy=userEntity.CreatedBy
                                   

                };
                try
                {
                    _unitOfWork.UserRepository.Insert(user);
                    _unitOfWork.Save();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return user.Id;
            }
        }
        public bool UpdateUser(long userId, UserEntity userEntity)
        {
            var success = false;

            //MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            //byte[] bs = Encoding.UTF8.GetBytes(userEntity.Password);
            //bs = x.ComputeHash(bs);
            //StringBuilder pwd = new StringBuilder();
            //foreach (byte b in bs)
            //{
            //    pwd.Append(b.ToString("x2").ToUpperInvariant());
            //}
            string pwd = CryptorEngine.Encrypt(userEntity.Password, true);
            if (userEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {
                       // user.UserId = userEntity.UserId;
                        user.Email = userEntity.Email;
                        user.Password = pwd.ToString();
                        user.FirstName = userEntity.FirstName;
                        //user.Email = userEntity.Email,
                        //Password = userEntity.Password,
                        user.MiddelName = userEntity.MiddelName;
                        user.LastName = userEntity.LastName;
                        user.Address = userEntity.Address;
                        user.PostalCode = userEntity.PostalCode;
                        user.PhoneNo = userEntity.PhoneNo;
                        user.Gender = userEntity.Gender;
                        user.ProfilePicture = userEntity.ProfilePicture;
                        user.IsActive = userEntity.IsActive;
                        user.RoleId = userEntity.RoleId;
                        user.CreatedBy = userEntity.CreatedBy;
                        try
                        {
                            _unitOfWork.UserRepository.Update(user);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                }
            }

            return success;
        }
        public UserEntity GetUserDetailByEmail(string email)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Email == email);
            if (user != null)
            {
                Mapper.CreateMap<UserProfile, UserEntity>();
                var userModel = Mapper.Map<UserProfile, UserEntity>(user);
                return userModel;
            }
            return null;
        }

        public bool ResetPassword(long userId,string email,string password)
        {
            //MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            //byte[] bs = Encoding.UTF8.GetBytes(password);
            //bs = x.ComputeHash(bs);
            //StringBuilder pwd = new StringBuilder();
            //foreach (byte b in bs)
            //{
            //    pwd.Append(b.ToString("x2").ToUpperInvariant());
            //}
            string pwd = CryptorEngine.Encrypt(password, true);
            var success = false;
            if(email!=null)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {                        
                        user.Password = pwd.ToString();
                        try
                        {
                            _unitOfWork.UserRepository.Update(user);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                    }
                }
            return success;
        }
    }

//------------------ Manage Hospital---------------->
    public class HospitalService : IHospitalServices
    {
        private readonly UnitOfWork _unitOfWork;
        public HospitalService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public HospitalEntity GetHospitalById(long HospitalId)
        {
            var Hospital = _unitOfWork.HospitalRepository.GetByID(HospitalId);
            if (Hospital != null)
            {
                Mapper.CreateMap<Hospital, HospitalEntity>();
                var productModel = Mapper.Map<Hospital, HospitalEntity>(Hospital);
                return productModel;
            }
            return null;
        }

        public bool HospitalStatus(int HospitalId)
        {
            var success = false;
            if (HospitalId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Hospital = _unitOfWork.HospitalRepository.GetByID(HospitalId);
                    if (Hospital != null)
                    {
                        Hospital.Id = HospitalId;
                        if (Hospital.IsAcctive == false)
                        {
                            Hospital.IsAcctive = true;
                        }
                        else
                        {
                            Hospital.IsAcctive = false;
                        }
                        try
                        {
                            _unitOfWork.HospitalRepository.Update(Hospital);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                }
            }
            return success;
        }

        public bool DeleteHospital(int HospitalId)
        {
            var success = false;
            if (HospitalId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Hospital = _unitOfWork.HospitalRepository.GetByID(HospitalId);
                    if (Hospital != null)
                    {

                        _unitOfWork.HospitalRepository.Delete(Hospital);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
        public IEnumerable<HospitalEntity> GetAllHospital()
        {
            var Hospitals = _unitOfWork.HospitalRepository.GetAll().ToList();
            if (Hospitals.Any())
            {
                Mapper.CreateMap<Hospital, HospitalEntity>();
                var CountrysModel = Mapper.Map<List<Hospital>, List<HospitalEntity>>(Hospitals);
                return CountrysModel;
            }
            return null;
        }
        public long CreateHospital(HospitalEntity ObjEntity)
        {
            using (var scope = new TransactionScope())
            {
                var Hospital = new Hospital
                {
                    HospitalName = ObjEntity.HospitalName,
                    Address=ObjEntity.Address,
                    Latitude=ObjEntity.Latitude,
                    Longitude=ObjEntity.Longitude,
                     IsAcctive=ObjEntity.IsAcctive,
                     CreatedBy=ObjEntity.CreatedBy


                };
                try
                {
                    _unitOfWork.HospitalRepository.Insert(Hospital);
                    _unitOfWork.Save();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return Hospital.Id;
            }
        }
        public bool UpdateHospital(long HospitalId, HospitalEntity ObjEntity)
        {
            //var obj= _unitOfWork.CountryRepository.SQLQuery<CountryEntity>("EXEC mySP @p1", new SqlParameter("@p1", "value"));
            var success = false;
            if (ObjEntity != null)
            {
                using (var scope = new TransactionScope())
                {                   
                    var Hospital = _unitOfWork.HospitalRepository.GetByID(HospitalId);
                    if (Hospital != null)
                    {
                        //.CountryId = CountryEntity.CountryId;
                        Hospital.HospitalName = ObjEntity.HospitalName;
                        // HospitalName = ObjEntity.HospitalName,
                        Hospital.Address = ObjEntity.Address;
                    Hospital.Latitude = ObjEntity.Latitude;
                    Hospital.Longitude = ObjEntity.Longitude;
                     Hospital.IsAcctive = ObjEntity.IsAcctive;
                        Hospital.CreatedBy = ObjEntity.CreatedBy;
                        try
                        {
                            _unitOfWork.HospitalRepository.Update(Hospital);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                }
            }

            return success;
        }
       public bool GetHospitalByLocation(decimal latitude, decimal longitude)
        {

            bool result = true;
            var user = _unitOfWork.HospitalRepository.Get(u =>u.Latitude==latitude && u.Longitude==longitude);
            if (user != null)
            {
                result = false;
            }               
            return result;
        }
    }
    //---------------------Manage Department------------------>
    public class DepartmentService : IDepartmentServices
    {
        private readonly UnitOfWork _unitOfWork;

        public DepartmentService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public DepartmentEntity GetDepartmentById(long DepartmentId)
        {
            var Dept = _unitOfWork.DepartmentRepository.GetByID(DepartmentId);
            if (Dept != null)
            {
                Mapper.CreateMap<Department, DepartmentEntity>();
                var productModel = Mapper.Map<Department, DepartmentEntity>(Dept);
                return productModel;
            }
            return null;
        }
        public bool DeleteDepartment(long DepartmentId)
        {
            var success = false;
            if (DepartmentId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Dept = _unitOfWork.DepartmentRepository.GetByID(DepartmentId);
                    if (Dept != null)
                    {

                        _unitOfWork.DepartmentRepository.Delete(Dept);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }
        public IEnumerable<DepartmentEntity> GetAllDepartment()
        {
            var Depts = _unitOfWork.DepartmentRepository.GetAll().ToList();
            if (Depts.Any())
            {
                Mapper.CreateMap<Department, DepartmentEntity>();
                var DeptsModel = Mapper.Map<List<Department>, List<DepartmentEntity>>(Depts);
                return DeptsModel;
            }
            return null;
        }
        public long CreateDepartment(DepartmentEntity ObjEntity)
        {
            using (var scope = new TransactionScope())
            {
                var Dept = new Department
                {
                    Name = ObjEntity.Name,
                    IsActive = ObjEntity.IsActive,
                    CreatedBy = ObjEntity.CreatedBy
                };
                try
                {
                    _unitOfWork.DepartmentRepository.Insert(Dept);
                    _unitOfWork.Save();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
                return Dept.Id;
            }
        }
        public bool UpdateDepartment(long DepartmentId, DepartmentEntity ObjEntity)
        {
            var success = false;
            if (ObjEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var Dept = _unitOfWork.DepartmentRepository.GetByID(DepartmentId);
                    if (Dept != null)
                    {
                        //.CityId = CityEntity.CityId;
                        Dept.Name = ObjEntity.Name;
                        Dept.IsActive = ObjEntity.IsActive;
                        Dept.CreatedBy = ObjEntity.CreatedBy;
                        try
                        {
                            _unitOfWork.DepartmentRepository.Update(Dept);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                }
            }

            return success;
        }
        public bool DepartmentStatus(long DepartmentId)
        {
            var success = false;
            if (DepartmentId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Dept = _unitOfWork.DepartmentRepository.GetByID(DepartmentId);
                    if (Dept != null)
                    {
                        Dept.Id = DepartmentId;
                        if (Dept.IsActive == false)
                        {
                            Dept.IsActive = true;
                        }
                        else
                        {
                            Dept.IsActive = false;
                        }
                        try
                        {
                            _unitOfWork.DepartmentRepository.Update(Dept);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                }
            }
            return success;
        }
    }
    //-------------------- Enquiry repo------------------>
    public class EnquiryService : IEnquiryServices
    {
        private readonly UnitOfWork _unitOfWork;
        public EnquiryService()
        {
            _unitOfWork = new UnitOfWork();
        }
        public EnquiryEntity GetEnquiryById(long EnquiryId)
        {
            var Enqiry = _unitOfWork.EnquiryRepository.GetByID(EnquiryId);
            if (Enqiry != null)
            {
                Mapper.CreateMap<Enqury, EnquiryEntity>();
                var productModel = Mapper.Map<Enqury, EnquiryEntity>(Enqiry);
                return productModel;
            }
            return null;
        }

        public IEnumerable<EnquiryEntity> GetAllEnquiry()
        {
            var Enqirys = _unitOfWork.EnquiryRepository.GetAll().ToList();
            if (Enqirys.Any())
            {
                Mapper.CreateMap<Enqury, EnquiryEntity>();
                var CMSPagesModel = Mapper.Map<List<Enqury>, List<EnquiryEntity>>(Enqirys);
                return CMSPagesModel;
            }
            return null;
        }
        public bool UpdateEnquiry(long EnquiryId, EnquiryEntity ObjEntity)
        {
            //using (var scope = new TransactionScope())
            //{
            //    var CMSPage = new Enqury
            //    {
            //        Name = CMSPageEntity.PageName
            //    };
            //    try
            //    {
            //        _unitOfWork.CMSPageRepository.Insert(CMSPage);
            //        _unitOfWork.Save();
            //        scope.Complete();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        return -1;
            //    }
            //    return CMSPage.CMSId;
            //}
            return false;
        }

      public  bool EnquiryStatus(int EnquiryId)
        {
            var success = false;
            if (EnquiryId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Enqry = _unitOfWork.EnquiryRepository.GetByID(EnquiryId);
                    if (Enqry != null)
                    {
                        Enqry.Id = EnquiryId;
                        if (Enqry.IsAnswered == false)
                        {
                            Enqry.IsAnswered = true;
                        }
                        else
                        {
                            Enqry.IsAnswered = false;
                        }
                        try
                        {
                            _unitOfWork.EnquiryRepository.Update(Enqry);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                }
            }
            return success;
        }
    }
    //-------------------- DDL Role------------------>
    public class RoleService : IRoleServices
    {
        private readonly UnitOfWork _unitOfWork;
        public RoleService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public  IEnumerable<UserRole> GetAllRole()
        {
            var userrole = _unitOfWork.UserRoleRepository.GetAll().ToList();
            if (userrole.Any())
            {
                return userrole;
            }
            return null;
        }
    }
    //---------------------------------- Hospital Doctor Mapping--------------------->
    public class HospitalDoctormap:IHospitalDoctorMapServices
    {
        private readonly UnitOfWork _unitOfWork;
        public HospitalDoctormap()
        {
            _unitOfWork = new UnitOfWork();
        }
      public  bool CreateHDocMapping(long DocId, HospitalDoctorEntity HDEntity)
        {
            var success = false;
            if (HDEntity != null)
            {
                using (var scope = new TransactionScope())
                {                    
                    var Dept = new HospitalDoctorMapping

                    {                     
                      HospitalId = HDEntity.HospitalId,
                      DoctorId = DocId,
                      DepartmentId = HDEntity.DepartmentId
                };
                        try
                        {
                            _unitOfWork.HospitalDoctorMappingRepository.Insert(Dept);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }
                }
            

            return success;

        }
    }

    public class HospitalDeptmap : IHospitalDeptMapServices
    {
        private readonly UnitOfWork _unitOfWork;
        public HospitalDeptmap()
        {
            _unitOfWork = new UnitOfWork();
        }
        public bool CreateHDeptMapping(long DepId, HospitalDeptEntity HDeptEntity)
        {
            HealthCareEntities _Context = new HealthCareEntities();
            var success = false;
            if (HDeptEntity != null)
            {                        
                    using (var scope = new TransactionScope())
                    {
                        var Dept = new HospitalDepartmentMapping

                        {
                            HospitalId = HDeptEntity.HospitalId,
                            DepartmentId = DepId
                        };
                        try
                        {
                            _unitOfWork.HospitalDepartmentMappingRepository.Insert(Dept);
                            _unitOfWork.Save();
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return success;
                        }
                        success = true;
                    }                
               
            }


            return success;

        }

        public List<DeptDdlEntity> GetAllDeptByHospitalId(long HospitalId)
        {
           //  HealthCareEntities Context = new HealthCareEntities();
        var AllDept = _unitOfWork.DepartmentRepository.GetAll().ToList();
            var dept = _unitOfWork.HospitalDepartmentMappingRepository.GetManyQueryable(m => m.HospitalId == HospitalId).ToList();
            var Depart = (from val in AllDept
                          join abc in dept on val.Id equals abc.DepartmentId
                          select new DeptDdlEntity
                          {
                DeptId= abc.DepartmentId,
                DeptName=val.Name
                          }).ToList();


            


            if (Depart.Any())
            {
                //Mapper.CreateMap<HospitalDepartmentMapping, HospitalDeptEntity>();
                ////Mapper.CreateMap<User, UserEntity>().ForMember(user => user.UserStatus,
                ////            map => map.MapFrom(entity => entity.Status));
                //var usersModel = Mapper.Map<List<HospitalDepartmentMapping>, List<HospitalDeptEntity>>(dept);
                return Depart;
            }
            return null;
        }

      public  IEnumerable<DdlHospitalDepartmentList> GetAllHospitalDeptmapping()
        {
            var AllDept = _unitOfWork.DepartmentRepository.GetAll().ToList();
            var Hospitals = _unitOfWork.HospitalRepository.GetAll().ToList();
            var users = _unitOfWork.HospitalDepartmentMappingRepository.GetAll().ToList();
            if (users.Any())
            {
                //Mapper.CreateMap<HospitalDepartmentMapping, HospitalDeptEntity>();
                ////Mapper.CreateMap<User, UserEntity>().ForMember(user => user.UserStatus,
                ////            map => map.MapFrom(entity => entity.Status));
                //var usersModel = Mapper.Map<List<HospitalDepartmentMapping>, List<HospitalDeptEntity>>(users);
                var Depart = (from val in AllDept
                              join abc in users on val.Id equals abc.DepartmentId
                              join cd in Hospitals on abc.HospitalId equals cd.Id
                              select new DdlHospitalDepartmentList
                              {
                                  DeptId = abc.DepartmentId,
                                  DeptName = val.Name,
                                  HospitalId=abc.HospitalId,
                                  HospitalName=cd.HospitalName
                              }).ToList();
                return Depart;
            }
            return null;
        }
    }
    
                  
    //public class AdverstisementService : IAdverstisementServices
    //{
    //    private readonly UnitOfWork _unitOfWork;
    //    public AdverstisementService()
    //    {
    //        _unitOfWork = new UnitOfWork();
    //    }
    //    public AdverstisementEntity GetAdverstisementById(int AdverstisementId)
    //    {
    //        var Adverstisement = _unitOfWork.AdverstisementRepository.GetByID(AdverstisementId);
    //        if (Adverstisement != null)
    //        {
    //            Mapper.CreateMap<Adverstisement, AdverstisementEntity>();
    //            var productModel = Mapper.Map<Adverstisement, AdverstisementEntity>(Adverstisement);
    //            return productModel;
    //        }
    //        return null;
    //    }
    //    public bool DeleteAdvertisement(int AdverstisementId)
    //    {
    //        var success = false;
    //        if (AdverstisementId > 0)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var Adverstisement = _unitOfWork.AdverstisementRepository.GetByID(AdverstisementId);
    //                if (Adverstisement != null)
    //                {

    //                    _unitOfWork.AdverstisementRepository.Delete(Adverstisement);
    //                    _unitOfWork.Save();
    //                    scope.Complete();
    //                    success = true;
    //                }
    //            }
    //        }
    //        return success;
    //    }
    //    public IEnumerable<AdverstisementEntity> GetAllAdverstisement()
    //    {
    //        var Adverstisements = _unitOfWork.AdverstisementRepository.GetAll().ToList();
    //        if (Adverstisements.Any())
    //        {
    //            Mapper.CreateMap<Adverstisement, AdverstisementEntity>();
    //            var AdverstisementsModel = Mapper.Map<List<Adverstisement>, List<AdverstisementEntity>>(Adverstisements);
    //            return AdverstisementsModel;
    //        }
    //        return null;
    //    }
    //    public int CreateAdverstisement(AdverstisementEntity AdverstisementEntity)
    //    {
    //        using (var scope = new TransactionScope())
    //        {
    //            var Adverstisement = new Adverstisement
    //            {
    //                AdvText = AdverstisementEntity.AdvText
    //            };
    //            try
    //            {
    //                _unitOfWork.AdverstisementRepository.Insert(Adverstisement);
    //                _unitOfWork.Save();
    //                scope.Complete();
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                return -1;
    //            }
    //            return Adverstisement.AdvId;
    //        }
    //    }
    //    public bool UpdateAdverstisement(int AdverstisementId, AdverstisementEntity AdverstisementEntity)
    //    {
    //        var success = false;
    //        if (AdverstisementEntity != null)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var Adverstisement = _unitOfWork.AdverstisementRepository.GetByID(AdverstisementId);
    //                if (Adverstisement != null)
    //                {
    //                    //.AdverstisementId = AdverstisementEntity.AdverstisementId;
    //                    Adverstisement.AdvText = AdverstisementEntity.AdvText;
    //                    try
    //                    {
    //                        _unitOfWork.AdverstisementRepository.Update(Adverstisement);
    //                        _unitOfWork.Save();
    //                        scope.Complete();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        Console.WriteLine(ex.Message);
    //                        return success;
    //                    }
    //                    success = true;
    //                }
    //            }
    //        }

    //        return success;
    //    }
    //}
    //public class EmailTemplatesService : IEmailTemplatesServices
    //{
    //    private readonly UnitOfWork _unitOfWork;
    //    public EmailTemplatesService()
    //    {
    //        _unitOfWork = new UnitOfWork();
    //    }
    //    public EmailTemplatesEntity GetEmailTemplatesById(int EmailTemplatesId)
    //    {
    //        var EmailTemplates = _unitOfWork.EmailTemplateRepository.GetByID(EmailTemplatesId);
    //        if (EmailTemplates != null)
    //        {
    //            Mapper.CreateMap<EmailTemplate, EmailTemplatesEntity>();
    //            var productModel = Mapper.Map<EmailTemplate, EmailTemplatesEntity>(EmailTemplates);
    //            return productModel;
    //        }
    //        return null;
    //    }
    //    public bool DeleteEmailTemplates(int EmailTemplatesId)
    //    {
    //        var success = false;
    //        if (EmailTemplatesId > 0)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var EmailTemplates = _unitOfWork.EmailTemplateRepository.GetByID(EmailTemplatesId);
    //                if (EmailTemplates != null)
    //                {

    //                    _unitOfWork.EmailTemplateRepository.Delete(EmailTemplates);
    //                    _unitOfWork.Save();
    //                    scope.Complete();
    //                    success = true;
    //                }
    //            }
    //        }
    //        return success;
    //    }
    //    public IEnumerable<EmailTemplatesEntity> GetAllEmailTemplates()
    //    {
    //        var EmailTemplatess = _unitOfWork.EmailTemplateRepository.GetAll().ToList();
    //        if (EmailTemplatess.Any())
    //        {
    //            Mapper.CreateMap<EmailTemplate, EmailTemplatesEntity>();
    //            var EmailTemplatessModel = Mapper.Map<List<EmailTemplate>, List<EmailTemplatesEntity>>(EmailTemplatess);
    //            return EmailTemplatessModel;
    //        }
    //        return null;
    //    }
    //    public int CreateEmailTemplates(EmailTemplatesEntity EmailTemplatesEntity)
    //    {
    //        using (var scope = new TransactionScope())
    //        {
    //            var EmailTemplates = new EmailTemplate
    //            {
    //                TempName = EmailTemplatesEntity.TempName
    //            };
    //            try
    //            {
    //                _unitOfWork.EmailTemplateRepository.Insert(EmailTemplates);
    //                _unitOfWork.Save();
    //                scope.Complete();
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                return -1;
    //            }
    //            return EmailTemplates.TempId;
    //        }
    //    }
    //    public bool UpdateEmailTemplates(int EmailTemplatesId, EmailTemplatesEntity EmailTemplatesEntity)
    //    {
    //        var success = false;
    //        if (EmailTemplatesEntity != null)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var EmailTemplates = _unitOfWork.EmailTemplateRepository.GetByID(EmailTemplatesId);
    //                if (EmailTemplates != null)
    //                {
    //                    //.EmailTemplatesId = EmailTemplatesEntity.EmailTemplatesId;
    //                    EmailTemplates.TempName = EmailTemplatesEntity.TempName;
    //                    EmailTemplates.Content = EmailTemplatesEntity.Content;

    //                    try
    //                    {
    //                        _unitOfWork.EmailTemplateRepository.Update(EmailTemplates);
    //                        _unitOfWork.Save();
    //                        scope.Complete();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        Console.WriteLine(ex.Message);
    //                        return success;
    //                    }
    //                    success = true;
    //                }
    //            }
    //        }

    //        return success;
    //    }
    //}
    //public class SportsService : ISportServices
    //{
    //    private readonly UnitOfWork _unitOfWork;
    //    public SportsService()
    //    {
    //        _unitOfWork = new UnitOfWork();
    //    }
    //    public SportEntity GetSportById(int SportsId)
    //    {
    //        var Sports = _unitOfWork.SportRepository.GetByID(SportsId);
    //        if (Sports != null)
    //        {
    //            Mapper.CreateMap<Sport, SportEntity>();
    //            var productModel = Mapper.Map<Sport, SportEntity>(Sports);
    //            return productModel;
    //        }
    //        return null;
    //    }
    //    public bool DeleteSport(int SportsId)
    //    {
    //        var success = false;
    //        if (SportsId > 0)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var Sports = _unitOfWork.SportRepository.GetByID(SportsId);
    //                if (Sports != null)
    //                {

    //                    _unitOfWork.SportRepository.Delete(Sports);
    //                    _unitOfWork.Save();
    //                    scope.Complete();
    //                    success = true;
    //                }
    //            }
    //        }
    //        return success;
    //    }
    //    public IEnumerable<SportEntity> GetAllSport()
    //    {
    //        var Sportss = _unitOfWork.SportRepository.GetAll().ToList();
    //        if (Sportss.Any())
    //        {
    //            Mapper.CreateMap<Sport, SportEntity>();
    //            var SportssModel = Mapper.Map<List<Sport>, List<SportEntity>>(Sportss);
    //            return SportssModel;
    //        }
    //        return null;
    //    }
    //    public int CreateSport(SportEntity SportsEntity)
    //    {
    //        using (var scope = new TransactionScope())
    //        {
    //            var Sports = new Sport
    //            {
    //                SportName = SportsEntity.SportName
    //            };
    //            try
    //            {
    //                _unitOfWork.SportRepository.Insert(Sports);
    //                _unitOfWork.Save();
    //                scope.Complete();
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                return -1;
    //            }
    //            return Sports.SportId;
    //        }
    //    }
    //    public bool UpdateSport(int SportsId, SportEntity SportsEntity)
    //    {
    //        var success = false;
    //        if (SportsEntity != null)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var Sports = _unitOfWork.SportRepository.GetByID(SportsId);
    //                if (Sports != null)
    //                {
    //                    //.SportsId = SportsEntity.SportsId;
    //                    Sports.SportName = SportsEntity.SportName;
    //                    try
    //                    {
    //                        _unitOfWork.SportRepository.Update(Sports);
    //                        _unitOfWork.Save();
    //                        scope.Complete();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        Console.WriteLine(ex.Message);
    //                        return success;
    //                    }
    //                    success = true;
    //                }
    //            }
    //        }

    //        return success;
    //    }
    //}
    //public class SlidersService : ISliderServices
    //{
    //    private readonly UnitOfWork _unitOfWork;
    //    public SlidersService()
    //    {
    //        _unitOfWork = new UnitOfWork();
    //    }
    //    public SlidersEntity GetSliderById(int SlidersId)
    //    {
    //        var Sliders = _unitOfWork.SliderRepository.GetByID(SlidersId);
    //        if (Sliders != null)
    //        {
    //            Mapper.CreateMap<Slider, SlidersEntity>();
    //            var productModel = Mapper.Map<Slider, SlidersEntity>(Sliders);
    //            return productModel;
    //        }
    //        return null;
    //    }
    //    public bool DeleteSlider(int SlidersId)
    //    {
    //        var success = false;
    //        if (SlidersId > 0)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var Sliders = _unitOfWork.SliderRepository.GetByID(SlidersId);
    //                if (Sliders != null)
    //                {

    //                    _unitOfWork.SliderRepository.Delete(Sliders);
    //                    _unitOfWork.Save();
    //                    scope.Complete();
    //                    success = true;
    //                }
    //            }
    //        }
    //        return success;
    //    }
    //    public IEnumerable<SlidersEntity> GetAllSlider()
    //    {
    //        var Sliderss = _unitOfWork.SliderRepository.GetAll().ToList();
    //        if (Sliderss.Any())
    //        {
    //            Mapper.CreateMap<Slider, SlidersEntity>();
    //            var SliderssModel = Mapper.Map<List<Slider>, List<SlidersEntity>>(Sliderss);
    //            return SliderssModel;
    //        }
    //        return null;
    //    }
    //    public int CreateSlider(SlidersEntity SlidersEntity)
    //    {
    //        using (var scope = new TransactionScope())
    //        {
    //            var Sliders = new Slider
    //            {
    //                SliderFor = SlidersEntity.SliderFor
    //            };
    //            try
    //            {
    //                _unitOfWork.SliderRepository.Insert(Sliders);
    //                _unitOfWork.Save();
    //                scope.Complete();
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                return -1;
    //            }
    //            return Sliders.SliderId;
    //        }
    //    }
    //    public bool UpdateSlider(int SlidersId, SlidersEntity SlidersEntity)
    //    {
    //        var success = false;
    //        if (SlidersEntity != null)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var Sliders = _unitOfWork.SliderRepository.GetByID(SlidersId);
    //                if (Sliders != null)
    //                {
    //                    //.SlidersId = SlidersEntity.SlidersId;
    //                    Sliders.SliderFor = SlidersEntity.SliderFor;
    //                    try
    //                    {
    //                        _unitOfWork.SliderRepository.Update(Sliders);
    //                        _unitOfWork.Save();
    //                        scope.Complete();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        Console.WriteLine(ex.Message);
    //                        return success;
    //                    }
    //                    success = true;
    //                }
    //            }
    //        }

    //        return success;
    //    }
    //}
    //public class EventService : IEventServices
    //{
    //    private readonly UnitOfWork _unitOfWork;
    //    public EventService()
    //    {
    //        _unitOfWork = new UnitOfWork();
    //    }
    //    public EventEntity GetEventById(int EventId)
    //    {
    //        var Event = _unitOfWork.EventRepository.GetByID(EventId);
    //        if (Event != null)
    //        {
    //            Mapper.CreateMap<Event, EventEntity>();
    //            var productModel = Mapper.Map<Event, EventEntity>(Event);
    //            return productModel;
    //        }
    //        return null;
    //    }
    //    public bool DeleteEvent(int EventId)
    //    {
    //        var success = false;
    //        if (EventId > 0)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var Event = _unitOfWork.EventRepository.GetByID(EventId);
    //                if (Event != null)
    //                {

    //                    _unitOfWork.EventRepository.Delete(Event);
    //                    _unitOfWork.Save();
    //                    scope.Complete();
    //                    success = true;
    //                }
    //            }
    //        }
    //        return success;
    //    }
    //    public IEnumerable<EventEntity> GetAllEvent()
    //    {
    //        var Events = _unitOfWork.EventRepository.GetAll().ToList();
    //        if (Events.Any())
    //        {
    //            Mapper.CreateMap<Event, EventEntity>();
    //            var EventsModel = Mapper.Map<List<Event>, List<EventEntity>>(Events);
    //            return EventsModel;
    //        }
    //        return null;
    //    }
    //    public int CreateEvent(EventEntity EventEntity)
    //    {
    //        using (var scope = new TransactionScope())
    //        {
    //            var Event = new Event
    //            {
    //               // AdvText = EventEntity.AdvText
    //            };
    //            try
    //            {
    //                _unitOfWork.EventRepository.Insert(Event);
    //                _unitOfWork.Save();
    //                scope.Complete();
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //                return -1;
    //            }
    //            return Event.EventId;
    //        }
    //    }
    //    public bool UpdateEvent(int EventId, EventEntity EventEntity)
    //    {
    //        var success = false;
    //        if (EventEntity != null)
    //        {
    //            using (var scope = new TransactionScope())
    //            {
    //                var Event = _unitOfWork.EventRepository.GetByID(EventId);
    //                if (Event != null)
    //                {
    //                    //.EventId = EventEntity.EventId;
    //                    //Event.AdvText = EventEntity.AdvText;
    //                    try
    //                    {
    //                        _unitOfWork.EventRepository.Update(Event);
    //                        _unitOfWork.Save();
    //                        scope.Complete();
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        Console.WriteLine(ex.Message);
    //                        return success;
    //                    }
    //                    success = true;
    //                }
    //            }
    //        }

    //        return success;
    //    }
    //}

}