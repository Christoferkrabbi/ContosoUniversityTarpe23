﻿using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            //Teeb, kindlaks, et andmebaas thakse, või oleks olemas
            context.Database.EnsureCreated();

            //Kui õpliaste tabelis juba on õpilasi, siis väljub funktsioon
            if (context.Students.Any())
            {
                return;
            }

            //objekti õpilastega, mis lisatakse siis, kui õpilasi sisestatud ei ole
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
            //andmebaasi muudatused salvestatakse
            context.SaveChanges();

            if (context.Courses.Any())
            {
                return;
            }

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
            context.Courses.AddRange(courses);
            context.SaveChanges();


            if (context.Enrollments.Any()) { return; }

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
            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();

            if (context.Instructors.Any())
            {
                return;
            }

            var instructors = new Instructor[]
            {
                new Instructor {FirstMidName = "Ban", LastName = "ana", HireDate = DateTime.Parse("2042-04-19"), Mood = Mood.Darklord, VocationCredential = "Complicated", WorkYears = 42},
                new Instructor {FirstMidName = "Quantum", LastName = "Kiin", HireDate = DateTime.Parse("2012-04-19"), Mood = Mood.HighAF, VocationCredential = "Basement", WorkYears = 42},
                new Instructor {FirstMidName = "Owo", LastName = "UwU", HireDate = DateTime.Parse("2002-04-19"), Mood = Mood.Darklord, VocationCredential = "Professional Dissapointment", WorkYears = 9000},
                new Instructor {FirstMidName = "Cursed", LastName = "Child", HireDate = DateTime.Parse("2013-04-19"), Mood = Mood.HighAF, VocationCredential = "Professional child", WorkYears = 42},
                new Instructor {FirstMidName = "Pain", LastName = "Misery", HireDate = DateTime.Parse("2016-04-19"), Mood = Mood.Darklord, VocationCredential = "Complicated", WorkYears = 42},
                new Instructor {FirstMidName = "Zomer", LastName = "Bimpson", HireDate = DateTime.Parse("2001-04-19"), Mood = Mood.Darklord, VocationCredential = "Complicated", WorkYears = 42},
                new Instructor {FirstMidName = "Donald J", LastName = "Trumo", HireDate = DateTime.Parse("1991-04-19"), Mood = Mood.HighAF, VocationCredential = "Complicated", WorkYears = 42},
                new Instructor {FirstMidName = "God", LastName = "Help us", HireDate = DateTime.Parse("1992-02-19"), Mood = Mood.Anxious, VocationCredential = "Very Very Complicated", WorkYears = 365000000},
                new Instructor {FirstMidName = "This is", LastName = "Cursed", HireDate = DateTime.Parse("1592-01-12"), Mood = Mood.Anxious, VocationCredential = "Complicated", WorkYears = 42},

            };
            context.Instructors.AddRange(instructors);
            context.SaveChanges();


            if (context.Departments.Any())
            {
                return;
            }
            var departments = new Department[]
            {
                new Department {
                Name = "InfoTechnology",
                Budget = 0,
                StartDate = DateTime.Parse("2025/09/24"),
                Cigarettes = 15,
                DarkLord = "The darklord of the East \"Bimpson\" with the power of ****** at his fingertips he shows no mercy",
                InstructorID = 1
                },

                new Department {
                Name = "The dark arts",
                Budget = 15000000,
                StartDate = DateTime.Parse("0001/03/12"),
                Cigarettes = 15,
                DarkLord = "The darklord of the Northern mountains and of scandinavian decent \"Zomber\" he is a devil in human skin and the brother of Dun dun dun Homer simpson",
                InstructorID = 2
                },


                new Department {
                Name = "The US election",
                Budget = 1000000,
                StartDate = DateTime.Parse("2016/09/24"),
                Cigarettes = 15,
                DarkLord = "The darklord of the United States \"Trump\" with the power of nagging at is fingertips he never relents and will annoy you until you give in (To be honest he isnt that cool compared to the rest just saying)",
                InstructorID = 3
                }

            };
            context.Departments.AddRange(departments);
            context.SaveChanges();

        }
    }
}

