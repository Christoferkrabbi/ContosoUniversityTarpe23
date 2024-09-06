using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            //Teeb, kindaaks, et andmebaas thakse, või oleks olemas
            context.Database.EnsureCreated();

            //Kui õpliaste tabelis juba on õpilasi, siis väljub funktsioon
            if (context.Students.Any())
            {
                return;
            }

            //objekyi õpilastega, mis lisatakse siis, kui õpilasi sisestatud ei ole
            var students = new Student[]
            {
                new Student {FirstMidName ="Artjom", LastName="seep", EnrollmentDate=DateTime.Parse("2069-04-20")},
                new Student {FirstMidName ="Meredeith", LastName="Alonso", EnrollmentDate=DateTime.Parse("2002-09-01") },
                new Student {FirstMidName ="Christofer", LastName="Krabbi", EnrollmentDate=DateTime.Parse("2007-01-23") },
                new Student {FirstMidName ="Yan", LastName="Li", EnrollmentDate=DateTime.Parse("2065-07-21") },
                new Student {FirstMidName ="Allah", LastName="Norman", EnrollmentDate=DateTime.Parse("2034-04-30") },
                new Student {FirstMidName ="John", LastName="Marston", EnrollmentDate=DateTime.Parse("2053-06-03") },
                new Student {FirstMidName ="Dutch", LastName="Plans", EnrollmentDate=DateTime.Parse("2002-01-01") },
                new Student {FirstMidName ="Morgan", LastName="Free", EnrollmentDate=DateTime.Parse("2002-08-01") },
                new Student {FirstMidName ="James", LastName="Hetfield", EnrollmentDate=DateTime.Parse("2000-05-26") },
                new Student {FirstMidName ="Lars", LastName="Hammet", EnrollmentDate=DateTime.Parse("2016-06-08") },
            };
           
            // Iga õpilane lisatakse ükssaaval läbi forreach tsükli
            foreach (Student student in students) 
            {
                context.Students.Add(student);
            }
            //Ja andmebaasi muudatused salvestatakse
            context.SaveChanges();

            //Eelnev struktuur, kuid kursustega: \/
            var courses = new Course[]
            {
                new Course{CourseID =1050, Title ="Keemia", Credits=3 },
                new Course{CourseID =4022, Title ="Matemaatika", Credits=3 },
                new Course{CourseID =4041, Title ="Mikroökonoomia", Credits=3 },
                new Course{CourseID =1045, Title ="Kirjandus", Credits=4 },
                new Course{CourseID =1530, Title ="Calculus", Credits=4 },
                new Course{CourseID =2034, Title ="Kompositsioon", Credits=3 },
                new Course{CourseID =9086, Title ="Kirjandus", Credits=4 },
                new Course{CourseID =2075, Title ="Videomängude Ajalugu", Credits=1 },
                new Course{CourseID =3141, Title ="Muusika", Credits=1 },
                new Course{CourseID =9001, Title ="Videomängud", Credits=1 },

            };
            
            foreach (Course course in courses) 
            { 
                context.Courses.Add(course);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment {StudentID=1, CourseID=1050,Grade =Grade.A},
                new Enrollment {StudentID=2, CourseID=4022,Grade =Grade.C},
                new Enrollment {StudentID=3, CourseID=4041,Grade =Grade.A},
                new Enrollment {StudentID=4, CourseID=1045,Grade =Grade.F},
                new Enrollment {StudentID=5, CourseID=1530,Grade =Grade.B},
                new Enrollment {StudentID=6, CourseID=2034,Grade =Grade.A},
                new Enrollment {StudentID=7, CourseID=9086,Grade =Grade.F},
                new Enrollment {StudentID=8, CourseID=2075,Grade =Grade.B},
                new Enrollment {StudentID=9, CourseID=3141,Grade =Grade.B},
                new Enrollment {StudentID=10, CourseID=9001,Grade =Grade.D},
            };
            foreach (Enrollment enrollment in enrollments)
            {
                context.Enrollments.Add(enrollment);
            }
            context.SaveChanges();
        }
    }
}

