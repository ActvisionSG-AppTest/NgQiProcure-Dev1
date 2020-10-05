using QiProcureDemo.ApprovalRequests;
using QiProcureDemo.ProjectInstructions;
using QiProcureDemo.Emails;
using QiProcureDemo.Approvals;
using QiProcureDemo.Projects;
using QiProcureDemo.Accounts;
using QiProcureDemo.TeamMembers;
using QiProcureDemo.Teams;
using QiProcureDemo.SysStatuses;
using QiProcureDemo.ParamSettings;
using QiProcureDemo.Documents;
using QiProcureDemo.ServiceImages;
using QiProcureDemo.ServicePrices;
using QiProcureDemo.ServiceCategories;
using QiProcureDemo.Services;
using QiProcureDemo.SysRefs;
using QiProcureDemo.ReferenceTypes;
using QiProcureDemo.ProductPrices;
using QiProcureDemo.ProductCategories;
using QiProcureDemo.ProductImages;
using QiProcureDemo.Products;
using QiProcureDemo.Categories;									 				   
using Abp.IdentityServer4;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QiProcureDemo.Authorization.Delegation;
using QiProcureDemo.Authorization.Roles;
using QiProcureDemo.Authorization.Users;
using QiProcureDemo.Chat;
using QiProcureDemo.Editions;
using QiProcureDemo.Friendships;
using QiProcureDemo.MultiTenancy;
using QiProcureDemo.MultiTenancy.Accounting;
using QiProcureDemo.MultiTenancy.Payments;
using QiProcureDemo.Storage;

namespace QiProcureDemo.EntityFrameworkCore
{
    public class QiProcureDemoDbContext : AbpZeroDbContext<Tenant, Role, User, QiProcureDemoDbContext>, IAbpPersistedGrantDbContext
    {
		public virtual DbSet<ApprovalRequest> ApprovalRequests { get; set; }

        public virtual DbSet<ProjectInstruction> ProjectInstructions { get; set; }

        public virtual DbSet<Email> Emails { get; set; }

        public virtual DbSet<Approval> Approvals { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<TeamMember> TeamMembers { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<SysStatus> SysStatuses { get; set; }

        public virtual DbSet<ParamSetting> ParamSettings { get; set; }

        public virtual DbSet<Document> Documents { get; set; }

        public virtual DbSet<ServiceImage> ServiceImages { get; set; }

        public virtual DbSet<ServicePrice> ServicePrices { get; set; }

        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<SysRef> SysRefs { get; set; }

        public virtual DbSet<ReferenceType> ReferenceTypes { get; set; }

        public virtual DbSet<ProductPrice> ProductPrices { get; set; }

        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        public virtual DbSet<ProductImage> ProductImages { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Category> Categories { get; set; }
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public QiProcureDemoDbContext(DbContextOptions<QiProcureDemoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApprovalRequest>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });

			modelBuilder.Entity<ProjectInstruction>(p =>
			{
				p.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Email>(x =>
			{
				x.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Approval>(a =>
			{
				a.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Project>(p =>
			{
				p.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Account>(a =>
			{
				a.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<TeamMember>(t =>
			{
				t.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Team>(t =>
			{
				t.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<SysStatus>(s =>
			{
				s.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<ParamSetting>(p =>
			{
				p.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Document>(d =>
			{
				d.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<ServiceImage>(s =>
			{
				s.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<ServicePrice>(s =>
			{
				s.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<ServiceCategory>(s =>
			{
				s.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Service>(s =>
			{
				s.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<SysRef>(s =>
			{
				s.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<ReferenceType>(r =>
			{
				r.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<ProductPrice>(p =>
			{
				p.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<ProductCategory>(p =>
			{
				p.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<ProductImage>(p =>
			{
				p.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Product>(p =>
			{
				p.HasIndex(e => new { e.TenantId });
			});
			modelBuilder.Entity<Category>(c =>
			{
				c.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
