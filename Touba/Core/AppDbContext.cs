namespace Touba.Core;

public class AppDbContext : IdentityDbContext<UserEntity> {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<ContactInformationEntity> ContactInformation { get; set; }
    public virtual DbSet<MediaEntity> Media { get; set; }
    public virtual DbSet<ContentEntity> Content { get; set; }
    public virtual DbSet<OtpEntity> Otp { get; set; }
    public virtual DbSet<VisitorEntity> Visitor { get; set; }
    public virtual DbSet<StatisticEntity> Statistic { get; set; }
    public virtual DbSet<TenderEntity> Tender { get; set; }
    public virtual DbSet<LocationEntity> Locations { get; set; }
    public virtual DbSet<ProductEntity> Product { get; set; }
    public virtual DbSet<CategoryEntity> Category { get; set; }
    public virtual DbSet<TagEntity> Tags { get; set; }
    public virtual DbSet<SpecialityEntity> Specialities { get; set; }
    public virtual DbSet<FavoriteEntity> Categories { get; set; }
    public virtual DbSet<ColorEntity> Colors { get; set; }
    public virtual DbSet<ReferenceEntity> References { get; set; }
    public virtual DbSet<BrandEntity> BrandEntities { get; set; }
    public virtual DbSet<ContactInfoItemEntity> ContactInfoItems { get; set; }
    public virtual DbSet<BankTransaction> BankTransaction { get; set; }
    public virtual DbSet<Transaction> Transaction { get; set; }
    public virtual DbSet<ProjectEntity> Project { get; set; }
    public virtual DbSet<TutorialEntity> Tutorial { get; set; }
    public virtual DbSet<EventEntity> Event { get; set; }
    public virtual DbSet<Log> Log { get; set; }
    public virtual DbSet<AdEntity> Ad { get; set; }
    public virtual DbSet<CompanyEntity> Company { get; set; }
    public virtual DbSet<FormEntity> Forms { get; set; }
    public virtual DbSet<FormFieldEntity> FormFields { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        LogModelBuilderHelper.Build(builder.Entity<Log>());
        builder.Entity<Log>().ToTable("Log");
        DataSeeder.Seed(builder);
        base.OnModelCreating(builder);
    }
}