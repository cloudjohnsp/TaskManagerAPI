﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerAPI.Domain.Entities;

public class User
{
    public string Id { get; set; } = null!;
    public string NickName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public ICollection<TaskList> TasksLists { get; set; } = null!;

    public User()
    {}

    protected User(string id, string nickName, string password, DateTime createdAt, DateTime lastUpdatedAt, ICollection<TaskList> tasksLists)
    {
        Id = id;
        NickName = nickName;
        Password = password;
        CreatedAt = createdAt;
        LastUpdatedAt = lastUpdatedAt;
        TasksLists = tasksLists;
    }

    public static User Create(string nickName, string password)
        => new(
            Guid.NewGuid().ToString(),
            nickName,
            password,
            DateTime.Now,
            DateTime.Now,
            []
        );
}