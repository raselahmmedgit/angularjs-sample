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
        public IQueryable<Student> GetStudents
        {
            get
            {
                List<Student> _studentList = new List<Student>
                            {
                                new Student {Id=1, Name = "Rasel", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 5},
                                new Student {Id=2, Name = "Sohel", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 5},
                                new Student {Id=3, Name = "Safin", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 5},
                                new Student {Id=4, Name = "Mim", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 5},
                                new Student {Id=5, Name = "Bappi", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 5}
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

                return _studentList.AsQueryable();
            }
        }
        public IQueryable<Student> GetCustomGridStudents
        {
            get
            {
                List<Student> _studentList = new List<Student>
                            {
                                new Student {Id=1, Name = "Rasel", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 20},
                                new Student {Id=2, Name = "Sohel", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 20},
                                new Student {Id=3, Name = "Safin", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 20},
                                new Student {Id=4, Name = "Mim", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 20},
                                new Student {Id=5, Name = "Bappi", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 20},

                                new Student {Id=6, Name = "Rasel 2", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 20},
                                new Student {Id=7, Name = "Sohel 2", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 20},
                                new Student {Id=8, Name = "Safin 2", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 20},
                                new Student {Id=9, Name = "Mim 2", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 20},
                                new Student {Id=10, Name = "Bappi 2", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 20},

                                new Student {Id=11, Name = "Rasel 3", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 20},
                                new Student {Id=12, Name = "Sohel 3", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 20},
                                new Student {Id=13, Name = "Safin 3", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 20},
                                new Student {Id=14, Name = "Mim 3", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 20},
                                new Student {Id=15, Name = "Bappi 3", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 20},

                                new Student {Id=16, Name = "Rasel 4", EmailAddress = "rasel@mail.com", Mobile = "01911-555555", Total = 20},
                                new Student {Id=17, Name = "Sohel 4", EmailAddress = "sohel@mail.com", Mobile = "01911-666666", Total = 20},
                                new Student {Id=18, Name = "Safin 4", EmailAddress = "safin@mail.com", Mobile = "01911-777777", Total = 20},
                                new Student {Id=19, Name = "Mim 4", EmailAddress = "mim@mail.com", Mobile = "01911-888888", Total = 20},
                                new Student {Id=20, Name = "Bappi 4", EmailAddress = "bappi@mail.com", Mobile = "01911-999999", Total = 20}
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

                return _studentList.AsQueryable();
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