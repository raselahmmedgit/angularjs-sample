using lab.ngdemo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab.ngdemo.Models.CacheManagement
{
    public class StudentCacheHelper
    {
        public CacheManager Cache { get; set; }

        public StudentCacheHelper()
        {
            Cache = new CacheManager();
        }
        public List<Student> GetStudents
        {
            get
            {
                List<Student> _studentList = new List<Student>
                            {
                                new Student {Id=1, Name = "Rasel", EmailAddress = "rasel@mail.com", Mobile = "01911-555555"},
                                new Student {Id=2, Name = "Sohel", EmailAddress = "sohel@mail.com", Mobile = "01911-666666"},
                                new Student {Id=3, Name = "Safin", EmailAddress = "safin@mail.com", Mobile = "01911-777777"},
                                new Student {Id=4, Name = "Mim", EmailAddress = "mim@mail.com", Mobile = "01911-888888"},
                                new Student {Id=5, Name = "Bappi", EmailAddress = "bappi@mail.com", Mobile = "01911-999999"}
                            };
                string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
                string cacheKey = Constants.CacheKey.StudentList + appConstant;
                if (!CacheManager.ICache.IsSet(cacheKey))
                {
                    CacheManager.ICache.Set(cacheKey, _studentList);
                }
                else
                {
                    _studentList = CacheManager.ICache.Get(cacheKey) as List<Student>;
                }

                return _studentList;
            }
        }

        public Student GetStudent(int id)
        {
            var student = new Student();
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            string cacheKey = Constants.CacheKey.Student + appConstant;
            if (!CacheManager.ICache.IsSet(cacheKey))
            {
                student = GetStudents.FirstOrDefault(item => item.Id == id);
                CacheManager.ICache.Set(cacheKey, student);
            }
            else
            {
                student = CacheManager.ICache.Get(cacheKey) as Student;
            }
            return student;
        }

        public void AddStudent(Student student)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.StudentAdd + appConstant;

            List<Student> _studentList = new List<Student>();
            _studentList = GetStudents.ToList();
            _studentList.Add(student);

            string cacheKeyList = Constants.CacheKey.StudentList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _studentList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _studentList);
            }

        }

        public void EditStudent(Student student)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.StudentEdit + appConstant;

            List<Student> _studentList = new List<Student>();
            _studentList = GetStudents.ToList();
            var editStudent = _studentList.FirstOrDefault(item => item.Id == student.Id);
            editStudent = student;

            string cacheKeyList = Constants.CacheKey.StudentList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _studentList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _studentList);
            }

        }

        public void DeleteStudent(Student student)
        {
            string appConstant = SiteConfigurationReader.GetAppSettingsString(Constants.CacheKey.DefaultCacheLifeTimeInMinute);
            //string cacheKey = Constants.CacheKey.StudentDelete + appConstant;

            List<Student> _studentList = new List<Student>();
            _studentList = GetStudents.ToList();
            var studentRemove = _studentList.FirstOrDefault(item => item.Id == student.Id);
            _studentList.Remove(studentRemove);

            string cacheKeyList = Constants.CacheKey.StudentList + appConstant;
            if (CacheManager.ICache.IsSet(cacheKeyList))
            {
                CacheManager.ICache.Remove(cacheKeyList);
                CacheManager.ICache.Set(cacheKeyList, _studentList);
            }
            else
            {
                CacheManager.ICache.Set(cacheKeyList, _studentList);
            }

        }
    }
}