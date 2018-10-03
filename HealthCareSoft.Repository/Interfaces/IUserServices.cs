
using HealthCareSoft.Entity;
using HealthCareSoft.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Repository.Interfaces
{
    public interface IUserServices
    {
        UserEntity GetUserById(int userId);
        bool DeleteUser(int userId); 
        bool UserStatus(int userId);       
        IEnumerable<UserEntity> GetAllUsers();
        IEnumerable<UserEntity> GetAllSubAdmin();
        IEnumerable<UserEntity> GetAllDoctors();
        IEnumerable<UserEntity> GetAllPatients();
        long CreateUser(UserEntity userEntity);
        bool UpdateUser(long userId, UserEntity userEntity);
        UserEntity GetUserByName(string userId, string password);
        bool GetUserByUserName(string username);
        bool GetUserByEmail(string email);
        bool ResetPassword(long id, string Email, string password);
        UserEntity GetUserDetailByEmail(string email);
    }
    public interface IHospitalServices
    {
        HospitalEntity GetHospitalById(long HospitalId);
        bool DeleteHospital(int HospitalId);
        bool HospitalStatus(int HospitalId);
        IEnumerable<HospitalEntity> GetAllHospital();
        long CreateHospital(HospitalEntity ObjEntity);
        bool GetHospitalByLocation(decimal latitude,decimal longitude);
        bool UpdateHospital(long HospitalId, HospitalEntity ObjEntity);
    }
    public interface IDepartmentServices
    {
        DepartmentEntity GetDepartmentById(long DepartmentId);
        bool DeleteDepartment(long DepartmentId);
        bool DepartmentStatus(long DepartmentId);
        IEnumerable<DepartmentEntity> GetAllDepartment();
        long CreateDepartment(DepartmentEntity ObjEntity);
        bool UpdateDepartment(long DepartmentId, DepartmentEntity ObjEntity);
    }
    public interface IEnquiryServices
    {
        EnquiryEntity GetEnquiryById(long EnquiryId);      
        IEnumerable<EnquiryEntity> GetAllEnquiry();     
        bool UpdateEnquiry(long EnquiryId, EnquiryEntity ObjEntity);
        bool EnquiryStatus(int userId);
    }
    public interface IRoleServices
    {
        IEnumerable<UserRole> GetAllRole();
    }
    public interface IHospitalDoctorMapServices
    {
        bool CreateHDocMapping(long DocId, HospitalDoctorEntity HDEntity);
    }

    public interface IHospitalDeptMapServices
    {
        bool CreateHDeptMapping(long DepId, HospitalDeptEntity HDeptEntity);
        List<DeptDdlEntity> GetAllDeptByHospitalId(long HospitalId);
        IEnumerable<DdlHospitalDepartmentList> GetAllHospitalDeptmapping();
    }
    //public interface ISliderServices
    //{
    //    SlidersEntity GetSliderById(int SliderId);
    //    bool DeleteSlider(int SliderId);
    //    IEnumerable<SlidersEntity> GetAllSlider();
    //    int CreateSlider(SlidersEntity SliderEntity);
    //    bool UpdateSlider(int SliderId, SlidersEntity userEntity);
    //}
    //public interface ICMSPageServices
    //{
    //    CMSPageEntity GetCMSPageById(int CMSId);
    //    bool DeleteCMSPage(int CMSId);
    //    IEnumerable<CMSPageEntity> GetAllCMSPage();
    //    int CreateCMSPage(CMSPageEntity CMSPageEntity);
    //    bool UpdateCMSPage(int CMSId, CMSPageEntity userEntity);
    //}
    //public interface IEmailTemplatesServices
    //{
    //    EmailTemplatesEntity GetEmailTemplatesById(int TempId);
    //    bool DeleteEmailTemplates(int TempId);
    //    IEnumerable<EmailTemplatesEntity> GetAllEmailTemplates();
    //    int CreateEmailTemplates(EmailTemplatesEntity emailEntity);
    //    bool UpdateEmailTemplates(int TempId, EmailTemplatesEntity emailEntity);
    //}
    //public interface IAdverstisementServices
    //{
    //    AdverstisementEntity GetAdverstisementById(int AdvId);
    //    bool DeleteAdvertisement(int AdvId);
    //    IEnumerable<AdverstisementEntity> GetAllAdverstisement();
    //    int CreateAdverstisement(AdverstisementEntity AdverstisementEntity);
    //    bool UpdateAdverstisement(int AdvId, AdverstisementEntity AdvEntity);
    //}
    //public interface IEventServices
    //{
    //    EventEntity GetEventById(int eventid);
    //    bool DeleteEvent(int eventid);
    //    IEnumerable<EventEntity> GetAllEvent();
    //    int CreateEvent(EventEntity EventEntity);
    //    bool UpdateEvent(int eventid, EventEntity AdvEntity);
    //}
}
