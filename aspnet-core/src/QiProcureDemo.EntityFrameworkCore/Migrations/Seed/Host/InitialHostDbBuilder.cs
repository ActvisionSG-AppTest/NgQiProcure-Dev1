﻿using QiProcureDemo.EntityFrameworkCore;

namespace QiProcureDemo.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly QiProcureDemoDbContext _context;

        public InitialHostDbBuilder(QiProcureDemoDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
