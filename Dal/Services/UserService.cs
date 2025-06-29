﻿using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class UserService : IUser
    {
        private readonly DatabaseManager _context;
        public UserService(DatabaseManager db)
        {
            _context = db;
        }
        public User Create(User entity)
        {

            _context.Entry(entity).State = EntityState.Added;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error creating the user.", ex);
            }

            return entity;
        }



        public void Delete(int id)
        {
            var user = Read(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }


        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }


        public User Read(int id)
        {
            return _context.Users.Find(id);
        }


        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }

    }
}
