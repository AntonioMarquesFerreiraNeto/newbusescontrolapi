using BusesControl.Entities.Models.v1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Persistence.Contexts;

public class AppDbContext : IdentityDbContext<UserModel, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<BusModel> Buses { get; set; }
    public DbSet<ColorModel> Colors { get; set; }
    public DbSet<CustomerModel> Customers { get; set; }
    public DbSet<ContractModel> Contracts { get; set; }
    public DbSet<ContractDescriptionModel> ContractDescriptions { get; set; }
    public DbSet<ContractBusReplacementModel> ContractBusReplacements { get; set; }
    public DbSet<ContractDriverReplacementModel> ContractDriverReplacements { get; set; }
    public DbSet<CustomerContractModel> CustomersContract { get; set; }
    public DbSet<EmployeeModel> Employees { get; set; }
    public DbSet<SettingPanelModel> SettingsPanel { get; set; }
    public DbSet<ResetPasswordSecurityCodeModel> ResetPasswordsSecurityCode { get; set; }
    public DbSet<UserRegistrationQueueModel> UsersRegistrationQueue { get; set; }
    public DbSet<UserRegistrationSecurityCodeModel> UsersRegistrationSecurityCode { get; set; }
    public DbSet<FinancialModel> Financials { get; set; }
    public DbSet<InvoiceModel> Invoices { get; set; }
    public DbSet<InvoiceExpenseModel> InvoicesExpense { get; set; }
    public DbSet<SavedCardModel> SavedCards { get; set; }
    public DbSet<TerminationModel> Terminations { get; set; }
    public DbSet<WebhookModel> Webhooks { get; set; }
    public DbSet<SupplierModel> Suppliers { get; set; }
    public DbSet<NotificationModel> Notifications { get; set; }
    public DbSet<SupportTicketModel> SupportTickets { get; set; }
    public DbSet<SupportTicketMessageModel> SupportTicketMessages { get; set; }

    public override DbSet<UserModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserModel>(entity => {
            entity.ToTable("Users");
        });

        builder.Entity<IdentityRole<Guid>>(entity => {
            entity.ToTable("Roles");
        });

        builder.Entity<IdentityUserRole<Guid>>(entity => {
            entity.ToTable("UserRoles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(entity => {
            entity.ToTable("UserClaims");
        });

        builder.Entity<IdentityUserLogin<Guid>>(entity => {
            entity.ToTable("UserLogins");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(entity => {
            entity.ToTable("RoleClaims");
        });

        builder.Entity<IdentityUserToken<Guid>>(entity => {
            entity.ToTable("UserTokens");
        });

        builder.Entity<BusModel>()
            .HasOne(x => x.Color)
            .WithMany()
            .HasForeignKey(x => x.ColorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ContractModel>()
            .HasOne(x => x.SettingPanel)
            .WithMany()
            .HasForeignKey(x => x.SettingPanelId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<FinancialModel>()
            .HasOne(x => x.SettingPanel)
            .WithMany()
            .HasForeignKey(x => x.SettingPanelId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ContractModel>()
            .HasOne(x => x.ContractDescription)
            .WithMany()
            .HasForeignKey(x => x.ContractDescriptionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<SupportTicketModel>()
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ContractBusReplacementModel>()
            .HasOne(x => x.Contract)
            .WithMany()
            .HasForeignKey(x => x.ContractId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ContractDriverReplacementModel>()
            .HasOne(x => x.Contract)
            .WithMany()
            .HasForeignKey(x => x.ContractId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
