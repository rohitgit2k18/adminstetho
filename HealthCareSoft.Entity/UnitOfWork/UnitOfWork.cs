using HealthCareSoft.Entity;
using HealthCareSoft.Entity.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSoft.Entity.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        private HealthCareEntities _context = null;
        private DbContextTransaction Transaction { get; set; }
        private GenericRepository<UserProfile> _userRepository;
        private GenericRepository<Hospital> _hospitalRepository;
        private GenericRepository<Department> _departmentRepository;
         private GenericRepository<Enqury> _enquiryRepository;
        private GenericRepository<UserRole> _userRoleRepository;
        private GenericRepository<HospitalDoctorMapping> _hospitalDoctorRepository;
        private GenericRepository<HospitalDepartmentMapping> _hospitalDeptrepository; 
        //private GenericRepository<Adverstisement> _adverstisementRepository;
        //private GenericRepository<CMSPage> _cmspageRepository;
        //private GenericRepository<Slider> _sliderRepository;
        //private GenericRepository<Event> _eventRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new HealthCareEntities();
        }

        public UnitOfWork BeginTransaction()
        {
            Transaction = _context.Database.BeginTransaction();
            return this;
        }
        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<UserProfile> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepository<UserProfile>(_context);
                return _userRepository;
            }
        }
        public GenericRepository<Hospital> HospitalRepository
        {
            get
            {
                if (this._hospitalRepository == null)
                    this._hospitalRepository = new GenericRepository<Hospital>(_context);
                return _hospitalRepository;
            }
        }
        public GenericRepository<Department> DepartmentRepository
        {
            get
            {
                if (this._departmentRepository == null)
                    this._departmentRepository = new GenericRepository<Department>(_context);
                return _departmentRepository;
            }
        }

        public GenericRepository<Enqury> EnquiryRepository
        {
            get
            {
                if (this._enquiryRepository == null)
                    this._enquiryRepository = new GenericRepository<Enqury>(_context);
                return _enquiryRepository;
            }
        }
        public GenericRepository<UserRole> UserRoleRepository
        {
            get
            {
                if (this._userRoleRepository == null)
                    this._userRoleRepository = new GenericRepository<UserRole>(_context);
                return _userRoleRepository;
            }
        }
        public GenericRepository<HospitalDoctorMapping> HospitalDoctorMappingRepository
        {
            get
            {
                if (this._hospitalDoctorRepository == null)
                    this._hospitalDoctorRepository = new GenericRepository<HospitalDoctorMapping>(_context);
                return _hospitalDoctorRepository;
            }
        }

        public GenericRepository<HospitalDepartmentMapping> HospitalDepartmentMappingRepository
        {
            get
            {
                if (this._hospitalDeptrepository == null)
                    this._hospitalDeptrepository = new GenericRepository<HospitalDepartmentMapping>(_context);
                return _hospitalDeptrepository;
            }
        }
       
        
       
        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }
        public bool EndTransaction()
        {
            try
            {
                _context.SaveChanges();
                Transaction.Commit();
            }
            catch (DbEntityValidationException dbEx)
            {
                // add your exception handling code here
            }
            return true;
        }

        public void RollBack()
        {
            Transaction.Rollback();
            Dispose();
        }
        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}
