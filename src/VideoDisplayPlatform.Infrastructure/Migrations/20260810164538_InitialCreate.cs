using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoDisplayPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_Tenants",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TenantName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    TimeZoneId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeactivatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_Tenants", x => x.TenantId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PermissionCode = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermissionName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModuleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_Permissions", x => x.PermissionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceGroups",
                columns: table => new
                {
                    DeviceGroupId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    ParentDeviceGroupId = table.Column<long>(type: "bigint", nullable: true),
                    GroupCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GroupPath = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LevelNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceGroups", x => x.DeviceGroupId);
                    table.ForeignKey(
                        name: "FK_core_DeviceGroups_core_DeviceGroups_ParentDeviceGroupId",
                        column: x => x.ParentDeviceGroupId,
                        principalTable: "core_DeviceGroups",
                        principalColumn: "DeviceGroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_core_DeviceGroups_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_FirmwareVersions",
                columns: table => new
                {
                    FirmwareVersionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: true),
                    HardwareModel = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HardwareRevision = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VersionName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReleaseNotes = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileStorageKey = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    Sha256Hash = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: true),
                    IsApproved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsMandatory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReleasedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_FirmwareVersions", x => x.FirmwareVersionId);
                    table.ForeignKey(
                        name: "FK_core_FirmwareVersions_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_Tags",
                columns: table => new
                {
                    TagId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    TagName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_Tags", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_core_Tags_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_ApiClients",
                columns: table => new
                {
                    ApiClientId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    ClientName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientSecretHash = table.Column<byte[]>(type: "varbinary(64)", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RevokedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUsedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_ApiClients", x => x.ApiClientId);
                    table.ForeignKey(
                        name: "FK_iam_ApiClients_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_Roles",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: true),
                    RoleName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSystemRole = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RoleScopeKey = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_Roles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_iam_Roles_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsEmailVerified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPhoneVerified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false),
                    LockedUntilUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastLoginAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastLoginIp = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordChangedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastPasswordResetAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeactivatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_iam_Users_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceGroupClosure",
                columns: table => new
                {
                    AncestorGroupId = table.Column<long>(type: "bigint", nullable: false),
                    DescendantGroupId = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceGroupClosure", x => new { x.AncestorGroupId, x.DescendantGroupId });
                    table.ForeignKey(
                        name: "FK_core_DeviceGroupClosure_core_DeviceGroups_AncestorGroupId",
                        column: x => x.AncestorGroupId,
                        principalTable: "core_DeviceGroups",
                        principalColumn: "DeviceGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_core_DeviceGroupClosure_core_DeviceGroups_DescendantGroupId",
                        column: x => x.DescendantGroupId,
                        principalTable: "core_DeviceGroups",
                        principalColumn: "DeviceGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_core_DeviceGroupClosure_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_Devices",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceGroupId = table.Column<long>(type: "bigint", nullable: true),
                    DeviceCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SerialNumber = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BoardSerialNumber = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    EnvironmentType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    HardwareModel = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HardwareRevision = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Manufacturer = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirmwareVersion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OsVersion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TimeZoneId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    ProvisioningStatus = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    InstallationDateUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FirstSeenAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastSeenAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeactivatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastConfigVersion = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_core_Devices_core_DeviceGroups_DeviceGroupId",
                        column: x => x.DeviceGroupId,
                        principalTable: "core_DeviceGroups",
                        principalColumn: "DeviceGroupId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_core_Devices_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TokenHash = table.Column<byte[]>(type: "varbinary(64)", maxLength: 64, nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RevokedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReplacedByTokenId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceInfo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_RefreshTokens", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_iam_RefreshTokens_iam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    AssignedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_iam_RolePermissions_iam_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "iam_Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_iam_RolePermissions_iam_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "iam_Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_iam_RolePermissions_iam_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_UserDeviceGroupAccess",
                columns: table => new
                {
                    UserDeviceGroupAccessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceGroupId = table.Column<long>(type: "bigint", nullable: false),
                    CanView = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanControl = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanUpload = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanDeleteVideo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanManagePlaylist = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanSchedule = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanManageDevice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanViewLogs = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanRestart = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanConfigure = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_UserDeviceGroupAccess", x => x.UserDeviceGroupAccessId);
                    table.ForeignKey(
                        name: "FK_iam_UserDeviceGroupAccess_core_DeviceGroups_DeviceGroupId",
                        column: x => x.DeviceGroupId,
                        principalTable: "core_DeviceGroups",
                        principalColumn: "DeviceGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_iam_UserDeviceGroupAccess_iam_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_iam_UserDeviceGroupAccess_iam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_UserMfaMethods",
                columns: table => new
                {
                    UserMfaMethodId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    MethodType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    SecretEncrypted = table.Column<byte[]>(type: "longblob", nullable: true),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsVerified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VerifiedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUsedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_UserMfaMethods", x => x.UserMfaMethodId);
                    table.ForeignKey(
                        name: "FK_iam_UserMfaMethods_iam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_UserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    AssignedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_iam_UserRoles_iam_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "iam_Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_iam_UserRoles_iam_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_iam_UserRoles_iam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_UserTokens",
                columns: table => new
                {
                    UserTokenId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TokenType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    TokenHash = table.Column<byte[]>(type: "varbinary(64)", maxLength: 64, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedFromIp = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_UserTokens", x => x.UserTokenId);
                    table.ForeignKey(
                        name: "FK_iam_UserTokens_iam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "media_Videos",
                columns: table => new
                {
                    VideoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    VideoCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VideoName = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DurationMs = table.Column<long>(type: "bigint", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    FrameRate = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Codec = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContainerFormat = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VideoCodec = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AudioCodec = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AudioChannels = table.Column<short>(type: "smallint", nullable: true),
                    AudioSampleRate = table.Column<int>(type: "int", nullable: true),
                    BitrateKbps = table.Column<int>(type: "int", nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    Sha256Hash = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: true),
                    ThumbnailStorageKey = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreviewImageStorageKey = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_Videos", x => x.VideoId);
                    table.ForeignKey(
                        name: "FK_media_Videos_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_media_Videos_iam_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "playback_Playlists",
                columns: table => new
                {
                    PlaylistId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    PlaylistCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlaylistName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playback_Playlists", x => x.PlaylistId);
                    table.ForeignKey(
                        name: "FK_playback_Playlists_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_playback_Playlists_iam_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "audit_AuditLogs",
                columns: table => new
                {
                    AuditLogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    DeviceId = table.Column<long>(type: "bigint", nullable: true),
                    ActionType = table.Column<short>(type: "smallint", nullable: false),
                    EntityType = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityId = table.Column<long>(type: "bigint", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResultCode = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    OldValuesJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewValuesJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DetailsJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_AuditLogs", x => new { x.EventTimeUtc, x.AuditLogId });
                    table.ForeignKey(
                        name: "FK_audit_AuditLogs_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_audit_AuditLogs_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_audit_AuditLogs_iam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "command_DeviceCommands",
                columns: table => new
                {
                    CommandId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CommandType = table.Column<short>(type: "smallint", nullable: false),
                    CommandStatus = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdempotencyKey = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayloadJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AcknowledgedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RetryCount = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    MaxRetryCount = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    ErrorCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorMessage = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_command_DeviceCommands", x => x.CommandId);
                    table.ForeignKey(
                        name: "FK_command_DeviceCommands_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_command_DeviceCommands_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_command_DeviceCommands_iam_Users_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceCapabilities",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Supports4K = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsH264 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsH265 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsVP8 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsVP9 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsHdmi = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsRs485 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsCan = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Supports4G = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsWifi = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SupportsBluetooth = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MaxVideoWidth = table.Column<int>(type: "int", nullable: true),
                    MaxVideoHeight = table.Column<int>(type: "int", nullable: true),
                    MaxVideoBitrateKbps = table.Column<int>(type: "int", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceCapabilities", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_core_DeviceCapabilities_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceConfigurations",
                columns: table => new
                {
                    DeviceConfigurationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    ConfigurationJson = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfigHash = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsCurrent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CurrentDeviceKey = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceConfigurations", x => x.DeviceConfigurationId);
                    table.ForeignKey(
                        name: "FK_core_DeviceConfigurations_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_core_DeviceConfigurations_iam_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceCredentials",
                columns: table => new
                {
                    DeviceCredentialId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CredentialType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CredentialHash = table.Column<byte[]>(type: "varbinary(64)", maxLength: 64, nullable: false),
                    CertificateThumbprint = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IssuedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RevokedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsPrimary = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceCredentials", x => x.DeviceCredentialId);
                    table.ForeignKey(
                        name: "FK_core_DeviceCredentials_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceDisplays",
                columns: table => new
                {
                    DeviceDisplayId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayType = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    ResolutionWidth = table.Column<int>(type: "int", nullable: true),
                    ResolutionHeight = table.Column<int>(type: "int", nullable: true),
                    RefreshRate = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Orientation = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Brightness = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Volume = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    HdmiPort = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentInput = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsConnected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DisplaySerialNumber = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Manufacturer = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Model = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastDetectedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceDisplays", x => x.DeviceDisplayId);
                    table.ForeignKey(
                        name: "FK_core_DeviceDisplays_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceFirmware",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    FirmwareVersionId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    RequestedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PreviousVersion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentVersion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProgressPercent = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    ErrorCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorMessage = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceFirmware", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_core_DeviceFirmware_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_core_DeviceFirmware_core_FirmwareVersions_FirmwareVersionId",
                        column: x => x.FirmwareVersionId,
                        principalTable: "core_FirmwareVersions",
                        principalColumn: "FirmwareVersionId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceFirmwareHistory",
                columns: table => new
                {
                    DeviceFirmwareHistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    FirmwareVersionId = table.Column<long>(type: "bigint", nullable: false),
                    PreviousVersion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewVersion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    ProgressPercent = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FailureCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FailureMessage = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceFirmwareHistory", x => x.DeviceFirmwareHistoryId);
                    table.ForeignKey(
                        name: "FK_core_DeviceFirmwareHistory_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_core_DeviceFirmwareHistory_core_FirmwareVersions_FirmwareVer~",
                        column: x => x.FirmwareVersionId,
                        principalTable: "core_FirmwareVersions",
                        principalColumn: "FirmwareVersionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_core_DeviceFirmwareHistory_iam_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceLocalAccess",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    LocalWebEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LocalWebPort = table.Column<int>(type: "int", nullable: true),
                    RequireAuthentication = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowVideoUpload = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowVideoDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowDeviceControl = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowPlaylistManagement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowScheduleManagement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowConfiguration = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LanOnly = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VpnOnly = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastLocalAccessAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceLocalAccess", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_core_DeviceLocalAccess_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceLocations",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    SiteName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BuildingName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Floor = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Zone = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AddressLine = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceLocations", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_core_DeviceLocations_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceNetworkInterfaces",
                columns: table => new
                {
                    DeviceNetworkInterfaceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    InterfaceName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InterfaceType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    MacAddress = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IpAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ipv6Address = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CarrierName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SimIdentifier = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NetworkStatus = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    SignalStrength = table.Column<short>(type: "smallint", nullable: true),
                    NetworkLatencyMs = table.Column<int>(type: "int", nullable: true),
                    IsPrimary = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastConnectedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastSeenAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceNetworkInterfaces", x => x.DeviceNetworkInterfaceId);
                    table.ForeignKey(
                        name: "FK_core_DeviceNetworkInterfaces_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceSecurity",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CertificateThumbprint = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CertificateExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastAuthenticationAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastAuthenticationIp = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FailedAuthenticationCount = table.Column<int>(type: "int", nullable: false),
                    LockedUntilUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TamperEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TamperState = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastTamperAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SecureBootEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EncryptionEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceSecurity", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_core_DeviceSecurity_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceStorage",
                columns: table => new
                {
                    DeviceStorageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    StorageType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    MountPoint = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalBytes = table.Column<long>(type: "bigint", nullable: true),
                    UsedBytes = table.Column<long>(type: "bigint", nullable: true),
                    FreeBytes = table.Column<long>(type: "bigint", nullable: true),
                    HealthStatus = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    WearPercent = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    LastCheckedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceStorage", x => x.DeviceStorageId);
                    table.ForeignKey(
                        name: "FK_core_DeviceStorage_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "core_DeviceTags",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    AssignedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_core_DeviceTags", x => new { x.DeviceId, x.TagId });
                    table.ForeignKey(
                        name: "FK_core_DeviceTags_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_core_DeviceTags_core_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "core_Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_core_DeviceTags_iam_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_UserDeviceAccess",
                columns: table => new
                {
                    UserDeviceAccessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CanView = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanControl = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanUpload = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanDeleteVideo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanManagePlaylist = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanSchedule = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanManageDevice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanViewLogs = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanRestart = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanConfigure = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeny = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_UserDeviceAccess", x => x.UserDeviceAccessId);
                    table.ForeignKey(
                        name: "FK_iam_UserDeviceAccess_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_iam_UserDeviceAccess_iam_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_iam_UserDeviceAccess_iam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ops_Alerts",
                columns: table => new
                {
                    AlertId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: true),
                    AlertType = table.Column<short>(type: "smallint", nullable: false),
                    Severity = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FirstSeenAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastSeenAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ResolvedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DetailsJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AcknowledgedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    AcknowledgedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ResolvedByUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ops_Alerts", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_ops_Alerts_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ops_Alerts_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ops_Alerts_iam_Users_AcknowledgedByUserId",
                        column: x => x.AcknowledgedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ops_Alerts_iam_Users_ResolvedByUserId",
                        column: x => x.ResolvedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ops_DeviceEvents",
                columns: table => new
                {
                    EventId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    EventType = table.Column<short>(type: "smallint", nullable: false),
                    Severity = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayloadJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ops_DeviceEvents", x => new { x.EventTimeUtc, x.EventId });
                    table.ForeignKey(
                        name: "FK_ops_DeviceEvents_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ops_DeviceEvents_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ops_DeviceHeartbeatHistory",
                columns: table => new
                {
                    HeartbeatId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CpuPercent = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    MemoryPercent = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    StoragePercent = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TemperatureC = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NetworkType = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    SignalStrength = table.Column<short>(type: "smallint", nullable: true),
                    NetworkLatencyMs = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirmwareVersion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ops_DeviceHeartbeatHistory", x => new { x.EventTimeUtc, x.HeartbeatId });
                    table.ForeignKey(
                        name: "FK_ops_DeviceHeartbeatHistory_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ops_DeviceHeartbeatHistory_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sync_DeviceSyncState",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    LastServerConfigVersion = table.Column<long>(type: "bigint", nullable: false),
                    LastDeviceConfigVersion = table.Column<long>(type: "bigint", nullable: false),
                    LastServerPlaylistVersion = table.Column<long>(type: "bigint", nullable: false),
                    LastDevicePlaylistVersion = table.Column<long>(type: "bigint", nullable: false),
                    LastServerScheduleVersion = table.Column<long>(type: "bigint", nullable: false),
                    LastDeviceScheduleVersion = table.Column<long>(type: "bigint", nullable: false),
                    LastSyncedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SyncStatus = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    LastError = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sync_DeviceSyncState", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_sync_DeviceSyncState_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sync_SyncEvents",
                columns: table => new
                {
                    SyncEventId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventTimeUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Direction = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    EntityType = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityId = table.Column<long>(type: "bigint", nullable: true),
                    VersionNo = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    PayloadJson = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorMessage = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sync_SyncEvents", x => new { x.EventTimeUtc, x.SyncEventId });
                    table.ForeignKey(
                        name: "FK_sync_SyncEvents_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sync_SyncEvents_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "iam_UserSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RefreshTokenId = table.Column<long>(type: "bigint", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceInfo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastActivityAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RevokedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LogoutAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iam_UserSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_iam_UserSessions_iam_RefreshTokens_RefreshTokenId",
                        column: x => x.RefreshTokenId,
                        principalTable: "iam_RefreshTokens",
                        principalColumn: "RefreshTokenId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_iam_UserSessions_iam_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "media_VideoDeployments",
                columns: table => new
                {
                    VideoDeploymentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    VideoId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    DesiredVersion = table.Column<long>(type: "bigint", nullable: false),
                    InstalledVersion = table.Column<long>(type: "bigint", nullable: true),
                    ProgressPercent = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    LastError = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    RequestedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_VideoDeployments", x => x.VideoDeploymentId);
                    table.ForeignKey(
                        name: "FK_media_VideoDeployments_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_media_VideoDeployments_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_media_VideoDeployments_iam_Users_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_media_VideoDeployments_media_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "media_Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "media_VideoFiles",
                columns: table => new
                {
                    VideoFileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VideoId = table.Column<long>(type: "bigint", nullable: false),
                    StorageProvider = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    StorageBucket = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StorageKey = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContentType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    Sha256Hash = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: true),
                    IsPrimary = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_VideoFiles", x => x.VideoFileId);
                    table.ForeignKey(
                        name: "FK_media_VideoFiles_media_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "media_Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "media_VideoVersions",
                columns: table => new
                {
                    VideoVersionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VideoId = table.Column<long>(type: "bigint", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    StorageProvider = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    StorageBucket = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StorageKey = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    Sha256Hash = table.Column<byte[]>(type: "varbinary(32)", maxLength: 32, nullable: true),
                    DurationMs = table.Column<long>(type: "bigint", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    FrameRate = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Codec = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContainerFormat = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BitrateKbps = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_VideoVersions", x => x.VideoVersionId);
                    table.ForeignKey(
                        name: "FK_media_VideoVersions_iam_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_media_VideoVersions_media_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "media_Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "playback_PlaylistDevices",
                columns: table => new
                {
                    PlaylistId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    StartAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AssignedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playback_PlaylistDevices", x => new { x.PlaylistId, x.DeviceId });
                    table.ForeignKey(
                        name: "FK_playback_PlaylistDevices_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playback_PlaylistDevices_iam_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_playback_PlaylistDevices_playback_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "playback_Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "playback_PlaylistGroups",
                columns: table => new
                {
                    PlaylistId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceGroupId = table.Column<long>(type: "bigint", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    StartAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AssignedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playback_PlaylistGroups", x => new { x.PlaylistId, x.DeviceGroupId });
                    table.ForeignKey(
                        name: "FK_playback_PlaylistGroups_core_DeviceGroups_DeviceGroupId",
                        column: x => x.DeviceGroupId,
                        principalTable: "core_DeviceGroups",
                        principalColumn: "DeviceGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playback_PlaylistGroups_iam_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_playback_PlaylistGroups_playback_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "playback_Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "playback_PlaylistItems",
                columns: table => new
                {
                    PlaylistItemId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlaylistId = table.Column<long>(type: "bigint", nullable: false),
                    VideoId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    PlaybackMode = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    RepeatCount = table.Column<int>(type: "int", nullable: true),
                    PlayDurationMs = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playback_PlaylistItems", x => x.PlaylistItemId);
                    table.ForeignKey(
                        name: "FK_playback_PlaylistItems_media_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "media_Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_playback_PlaylistItems_playback_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "playback_Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "schedule_Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    ScheduleCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScheduleName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlaylistId = table.Column<long>(type: "bigint", nullable: false),
                    StartDateUtc = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDateUtc = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    DaysOfWeekMask = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    TimeZoneId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_Schedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_schedule_Schedules_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_schedule_Schedules_iam_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "iam_Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_schedule_Schedules_playback_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "playback_Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "command_CommandAttempts",
                columns: table => new
                {
                    CommandAttemptId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommandId = table.Column<long>(type: "bigint", nullable: false),
                    AttemptNo = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    AttemptStatus = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AckAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ErrorCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorMessage = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_command_CommandAttempts", x => x.CommandAttemptId);
                    table.ForeignKey(
                        name: "FK_command_CommandAttempts_command_DeviceCommands_CommandId",
                        column: x => x.CommandId,
                        principalTable: "command_DeviceCommands",
                        principalColumn: "CommandId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ops_DeviceCurrentStatus",
                columns: table => new
                {
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    ConnectivityStatus = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    LastHeartbeatAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastCommandAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastPlaybackAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CpuPercent = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    MemoryPercent = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    StoragePercent = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TemperatureC = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    UptimeSeconds = table.Column<long>(type: "bigint", nullable: true),
                    NetworkLatencyMs = table.Column<int>(type: "int", nullable: true),
                    SignalStrength = table.Column<short>(type: "smallint", nullable: true),
                    CurrentVideoId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentPlaylistId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentPlaylistItemId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentPlaybackPositionMs = table.Column<long>(type: "bigint", nullable: true),
                    CurrentPlaybackState = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    PlayerHealth = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    AgentHealth = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    ErrorCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorMessage = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConfigVersion = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ops_DeviceCurrentStatus", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_ops_DeviceCurrentStatus_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ops_DeviceCurrentStatus_media_Videos_CurrentVideoId",
                        column: x => x.CurrentVideoId,
                        principalTable: "media_Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ops_DeviceCurrentStatus_playback_PlaylistItems_CurrentPlayli~",
                        column: x => x.CurrentPlaylistItemId,
                        principalTable: "playback_PlaylistItems",
                        principalColumn: "PlaylistItemId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ops_DeviceCurrentStatus_playback_Playlists_CurrentPlaylistId",
                        column: x => x.CurrentPlaylistId,
                        principalTable: "playback_Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "playback_PlaybackSessions",
                columns: table => new
                {
                    PlaybackSessionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    VideoId = table.Column<long>(type: "bigint", nullable: true),
                    PlaylistId = table.Column<long>(type: "bigint", nullable: true),
                    PlaylistItemId = table.Column<long>(type: "bigint", nullable: true),
                    EndedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StartPositionMs = table.Column<long>(type: "bigint", nullable: true),
                    EndPositionMs = table.Column<long>(type: "bigint", nullable: true),
                    CompletionStatus = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    SourceType = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    ErrorCode = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorMessage = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playback_PlaybackSessions", x => new { x.StartedAtUtc, x.PlaybackSessionId });
                    table.ForeignKey(
                        name: "FK_playback_PlaybackSessions_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playback_PlaybackSessions_core_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "core_Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_playback_PlaybackSessions_media_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "media_Videos",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_playback_PlaybackSessions_playback_PlaylistItems_PlaylistIte~",
                        column: x => x.PlaylistItemId,
                        principalTable: "playback_PlaylistItems",
                        principalColumn: "PlaylistItemId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_playback_PlaybackSessions_playback_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "playback_Playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "schedule_ScheduleDevices",
                columns: table => new
                {
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_ScheduleDevices", x => new { x.ScheduleId, x.DeviceId });
                    table.ForeignKey(
                        name: "FK_schedule_ScheduleDevices_core_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "core_Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_schedule_ScheduleDevices_schedule_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "schedule_Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "schedule_ScheduleGroups",
                columns: table => new
                {
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceGroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_ScheduleGroups", x => new { x.ScheduleId, x.DeviceGroupId });
                    table.ForeignKey(
                        name: "FK_schedule_ScheduleGroups_core_DeviceGroups_DeviceGroupId",
                        column: x => x.DeviceGroupId,
                        principalTable: "core_DeviceGroups",
                        principalColumn: "DeviceGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_schedule_ScheduleGroups_schedule_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "schedule_Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_audit_AuditLogs_DeviceId",
                table: "audit_AuditLogs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_AuditLogs_TenantId",
                table: "audit_AuditLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_AuditLogs_UserId",
                table: "audit_AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_command_CommandAttempts_CommandId_AttemptNo",
                table: "command_CommandAttempts",
                columns: new[] { "CommandId", "AttemptNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_command_DeviceCommands_DeviceId_IdempotencyKey",
                table: "command_DeviceCommands",
                columns: new[] { "DeviceId", "IdempotencyKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_command_DeviceCommands_RequestedByUserId",
                table: "command_DeviceCommands",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_command_DeviceCommands_TenantId",
                table: "command_DeviceCommands",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceConfigurations_CreatedByUserId",
                table: "core_DeviceConfigurations",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceConfigurations_DeviceId_VersionNo",
                table: "core_DeviceConfigurations",
                columns: new[] { "DeviceId", "VersionNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceCredentials_CredentialHash",
                table: "core_DeviceCredentials",
                column: "CredentialHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceCredentials_DeviceId",
                table: "core_DeviceCredentials",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceDisplays_DeviceId",
                table: "core_DeviceDisplays",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceFirmware_FirmwareVersionId",
                table: "core_DeviceFirmware",
                column: "FirmwareVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceFirmwareHistory_DeviceId",
                table: "core_DeviceFirmwareHistory",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceFirmwareHistory_FirmwareVersionId",
                table: "core_DeviceFirmwareHistory",
                column: "FirmwareVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceFirmwareHistory_UpdatedByUserId",
                table: "core_DeviceFirmwareHistory",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceGroupClosure_DescendantGroupId",
                table: "core_DeviceGroupClosure",
                column: "DescendantGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceGroupClosure_TenantId",
                table: "core_DeviceGroupClosure",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceGroups_ParentDeviceGroupId",
                table: "core_DeviceGroups",
                column: "ParentDeviceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceGroups_TenantId_GroupCode",
                table: "core_DeviceGroups",
                columns: new[] { "TenantId", "GroupCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceNetworkInterfaces_DeviceId",
                table: "core_DeviceNetworkInterfaces",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_core_Devices_DeviceGroupId",
                table: "core_Devices",
                column: "DeviceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_core_Devices_TenantId_DeviceCode",
                table: "core_Devices",
                columns: new[] { "TenantId", "DeviceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_core_Devices_TenantId_DeviceGroupId_Status_DeviceId",
                table: "core_Devices",
                columns: new[] { "TenantId", "DeviceGroupId", "Status", "DeviceId" });

            migrationBuilder.CreateIndex(
                name: "IX_core_Devices_TenantId_SerialNumber",
                table: "core_Devices",
                columns: new[] { "TenantId", "SerialNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceStorage_DeviceId",
                table: "core_DeviceStorage",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceTags_AssignedByUserId",
                table: "core_DeviceTags",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_core_DeviceTags_TagId",
                table: "core_DeviceTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_core_FirmwareVersions_TenantId",
                table: "core_FirmwareVersions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_core_Tags_TenantId_TagName",
                table: "core_Tags",
                columns: new[] { "TenantId", "TagName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_core_Tenants_Status_TenantId",
                table: "core_Tenants",
                columns: new[] { "Status", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_core_Tenants_TenantCode",
                table: "core_Tenants",
                column: "TenantCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iam_ApiClients_TenantId_ClientId",
                table: "iam_ApiClients",
                columns: new[] { "TenantId", "ClientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iam_Permissions_PermissionCode",
                table: "iam_Permissions",
                column: "PermissionCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iam_RefreshTokens_TokenHash",
                table: "iam_RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iam_RefreshTokens_UserId",
                table: "iam_RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_RolePermissions_AssignedByUserId",
                table: "iam_RolePermissions",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_RolePermissions_PermissionId",
                table: "iam_RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_Roles_TenantId",
                table: "iam_Roles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserDeviceAccess_CreatedByUserId",
                table: "iam_UserDeviceAccess",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserDeviceAccess_DeviceId",
                table: "iam_UserDeviceAccess",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserDeviceAccess_UserId_DeviceId",
                table: "iam_UserDeviceAccess",
                columns: new[] { "UserId", "DeviceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserDeviceGroupAccess_CreatedByUserId",
                table: "iam_UserDeviceGroupAccess",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserDeviceGroupAccess_DeviceGroupId",
                table: "iam_UserDeviceGroupAccess",
                column: "DeviceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserDeviceGroupAccess_UserId_DeviceGroupId",
                table: "iam_UserDeviceGroupAccess",
                columns: new[] { "UserId", "DeviceGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserMfaMethods_UserId",
                table: "iam_UserMfaMethods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserRoles_AssignedByUserId",
                table: "iam_UserRoles",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserRoles_RoleId",
                table: "iam_UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_Users_TenantId_NormalizedEmail",
                table: "iam_Users",
                columns: new[] { "TenantId", "NormalizedEmail" });

            migrationBuilder.CreateIndex(
                name: "IX_iam_Users_TenantId_NormalizedUserName",
                table: "iam_Users",
                columns: new[] { "TenantId", "NormalizedUserName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserSessions_RefreshTokenId",
                table: "iam_UserSessions",
                column: "RefreshTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserSessions_UserId",
                table: "iam_UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserTokens_TokenHash",
                table: "iam_UserTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_iam_UserTokens_UserId",
                table: "iam_UserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_media_VideoDeployments_DeviceId",
                table: "media_VideoDeployments",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_media_VideoDeployments_RequestedByUserId",
                table: "media_VideoDeployments",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_media_VideoDeployments_TenantId",
                table: "media_VideoDeployments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_media_VideoDeployments_VideoId_DeviceId",
                table: "media_VideoDeployments",
                columns: new[] { "VideoId", "DeviceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_media_VideoFiles_VideoId",
                table: "media_VideoFiles",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_media_Videos_CreatedByUserId",
                table: "media_Videos",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_media_Videos_TenantId_VideoCode",
                table: "media_Videos",
                columns: new[] { "TenantId", "VideoCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_media_VideoVersions_CreatedByUserId",
                table: "media_VideoVersions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_media_VideoVersions_VideoId_VersionNo",
                table: "media_VideoVersions",
                columns: new[] { "VideoId", "VersionNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ops_Alerts_AcknowledgedByUserId",
                table: "ops_Alerts",
                column: "AcknowledgedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_Alerts_DeviceId",
                table: "ops_Alerts",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_Alerts_ResolvedByUserId",
                table: "ops_Alerts",
                column: "ResolvedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_Alerts_TenantId",
                table: "ops_Alerts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_DeviceCurrentStatus_CurrentPlaylistId",
                table: "ops_DeviceCurrentStatus",
                column: "CurrentPlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_DeviceCurrentStatus_CurrentPlaylistItemId",
                table: "ops_DeviceCurrentStatus",
                column: "CurrentPlaylistItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_DeviceCurrentStatus_CurrentVideoId",
                table: "ops_DeviceCurrentStatus",
                column: "CurrentVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_DeviceEvents_DeviceId",
                table: "ops_DeviceEvents",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_DeviceEvents_TenantId",
                table: "ops_DeviceEvents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_DeviceHeartbeatHistory_DeviceId",
                table: "ops_DeviceHeartbeatHistory",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ops_DeviceHeartbeatHistory_TenantId",
                table: "ops_DeviceHeartbeatHistory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaybackSessions_DeviceId",
                table: "playback_PlaybackSessions",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaybackSessions_PlaylistId",
                table: "playback_PlaybackSessions",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaybackSessions_PlaylistItemId",
                table: "playback_PlaybackSessions",
                column: "PlaylistItemId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaybackSessions_TenantId",
                table: "playback_PlaybackSessions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaybackSessions_VideoId",
                table: "playback_PlaybackSessions",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaylistDevices_AssignedByUserId",
                table: "playback_PlaylistDevices",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaylistDevices_DeviceId",
                table: "playback_PlaylistDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaylistGroups_AssignedByUserId",
                table: "playback_PlaylistGroups",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaylistGroups_DeviceGroupId",
                table: "playback_PlaylistGroups",
                column: "DeviceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaylistItems_PlaylistId_SortOrder",
                table: "playback_PlaylistItems",
                columns: new[] { "PlaylistId", "SortOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_playback_PlaylistItems_VideoId",
                table: "playback_PlaylistItems",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_Playlists_CreatedByUserId",
                table: "playback_Playlists",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_playback_Playlists_TenantId_PlaylistCode",
                table: "playback_Playlists",
                columns: new[] { "TenantId", "PlaylistCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_schedule_ScheduleDevices_DeviceId",
                table: "schedule_ScheduleDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_ScheduleGroups_DeviceGroupId",
                table: "schedule_ScheduleGroups",
                column: "DeviceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_Schedules_CreatedByUserId",
                table: "schedule_Schedules",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_Schedules_PlaylistId",
                table: "schedule_Schedules",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_Schedules_TenantId_ScheduleCode",
                table: "schedule_Schedules",
                columns: new[] { "TenantId", "ScheduleCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sync_SyncEvents_DeviceId",
                table: "sync_SyncEvents",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_sync_SyncEvents_TenantId",
                table: "sync_SyncEvents",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_AuditLogs");

            migrationBuilder.DropTable(
                name: "command_CommandAttempts");

            migrationBuilder.DropTable(
                name: "core_DeviceCapabilities");

            migrationBuilder.DropTable(
                name: "core_DeviceConfigurations");

            migrationBuilder.DropTable(
                name: "core_DeviceCredentials");

            migrationBuilder.DropTable(
                name: "core_DeviceDisplays");

            migrationBuilder.DropTable(
                name: "core_DeviceFirmware");

            migrationBuilder.DropTable(
                name: "core_DeviceFirmwareHistory");

            migrationBuilder.DropTable(
                name: "core_DeviceGroupClosure");

            migrationBuilder.DropTable(
                name: "core_DeviceLocalAccess");

            migrationBuilder.DropTable(
                name: "core_DeviceLocations");

            migrationBuilder.DropTable(
                name: "core_DeviceNetworkInterfaces");

            migrationBuilder.DropTable(
                name: "core_DeviceSecurity");

            migrationBuilder.DropTable(
                name: "core_DeviceStorage");

            migrationBuilder.DropTable(
                name: "core_DeviceTags");

            migrationBuilder.DropTable(
                name: "iam_ApiClients");

            migrationBuilder.DropTable(
                name: "iam_RolePermissions");

            migrationBuilder.DropTable(
                name: "iam_UserDeviceAccess");

            migrationBuilder.DropTable(
                name: "iam_UserDeviceGroupAccess");

            migrationBuilder.DropTable(
                name: "iam_UserMfaMethods");

            migrationBuilder.DropTable(
                name: "iam_UserRoles");

            migrationBuilder.DropTable(
                name: "iam_UserSessions");

            migrationBuilder.DropTable(
                name: "iam_UserTokens");

            migrationBuilder.DropTable(
                name: "media_VideoDeployments");

            migrationBuilder.DropTable(
                name: "media_VideoFiles");

            migrationBuilder.DropTable(
                name: "media_VideoVersions");

            migrationBuilder.DropTable(
                name: "ops_Alerts");

            migrationBuilder.DropTable(
                name: "ops_DeviceCurrentStatus");

            migrationBuilder.DropTable(
                name: "ops_DeviceEvents");

            migrationBuilder.DropTable(
                name: "ops_DeviceHeartbeatHistory");

            migrationBuilder.DropTable(
                name: "playback_PlaybackSessions");

            migrationBuilder.DropTable(
                name: "playback_PlaylistDevices");

            migrationBuilder.DropTable(
                name: "playback_PlaylistGroups");

            migrationBuilder.DropTable(
                name: "schedule_ScheduleDevices");

            migrationBuilder.DropTable(
                name: "schedule_ScheduleGroups");

            migrationBuilder.DropTable(
                name: "sync_DeviceSyncState");

            migrationBuilder.DropTable(
                name: "sync_SyncEvents");

            migrationBuilder.DropTable(
                name: "command_DeviceCommands");

            migrationBuilder.DropTable(
                name: "core_FirmwareVersions");

            migrationBuilder.DropTable(
                name: "core_Tags");

            migrationBuilder.DropTable(
                name: "iam_Permissions");

            migrationBuilder.DropTable(
                name: "iam_Roles");

            migrationBuilder.DropTable(
                name: "iam_RefreshTokens");

            migrationBuilder.DropTable(
                name: "playback_PlaylistItems");

            migrationBuilder.DropTable(
                name: "schedule_Schedules");

            migrationBuilder.DropTable(
                name: "core_Devices");

            migrationBuilder.DropTable(
                name: "media_Videos");

            migrationBuilder.DropTable(
                name: "playback_Playlists");

            migrationBuilder.DropTable(
                name: "core_DeviceGroups");

            migrationBuilder.DropTable(
                name: "iam_Users");

            migrationBuilder.DropTable(
                name: "core_Tenants");
        }
    }
}
