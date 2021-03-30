﻿using Budzet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Budzet.EFData
{
    public class Seed
    {
        public static void SeedUser(EFDataContext context)
        {
            if (!context.Users.Any())
            {
                if (!context.Users.Any())
                {
                    var cats = new List<AppUser>
                                {
                                    new AppUser
                                        {
                                            Name = "Гриша",
                                            DebitKredit=true,
                                            Tranіaction=new DateTime(2014, 4,2),
                                            Details="-2000 дол",
                                            Image="https://www.meme-arsenal.com/memes/06d6608a0dd7397bfd87a1d7e9213de9.jpg"
                                        },
                                    new AppUser
                                        {
                                            Name = "Катюха",
                                            DebitKredit=false,
                                            Tranіaction=new DateTime(2018, 10,13),
                                            Details="+500 дол",
                                            Image="https://new-magazine.ru/wp-content/uploads/2020/06/image-08-06-20-01-35-3.jpeg"
                                        },

                                };

                    foreach (var cat in cats)
                    {
                        context.Add(cat);
                        context.SaveChanges();
                    }
                }
            }
        }
       
    }
}