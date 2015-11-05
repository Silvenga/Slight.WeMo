using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Slight.WeMo.DataAccess;

namespace Slight.WeMo.DataAccess.Migrations
{
    [DbContext(typeof(WeMoContext))]
    partial class WeMoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta8-15964");

            modelBuilder.Entity("Slight.WeMo.Entities.Models.SwitchEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CurrentState");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .Annotation("MaxLength", 36);

                    b.Property<int>("OldState");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Slight.WeMo.Entities.Models.WeMoDevice", b =>
                {
                    b.Property<string>("DeviceId");

                    b.Property<string>("DeviceType")
                        .Annotation("MaxLength", 32);

                    b.Property<string>("FirmwareVersion")
                        .Annotation("MaxLength", 32);

                    b.Property<string>("FriendlyName")
                        .Annotation("MaxLength", 128);

                    b.Property<string>("Host")
                        .Annotation("MaxLength", 16);

                    b.Property<DateTime>("LastDetected");

                    b.Property<string>("Location")
                        .Annotation("MaxLength", 36);

                    b.Property<string>("MacAddress")
                        .Annotation("MaxLength", 12);

                    b.Property<string>("ModelName")
                        .Annotation("MaxLength", 32);

                    b.Property<string>("ModelNumber")
                        .Annotation("MaxLength", 32);

                    b.Property<int>("Port");

                    b.Property<string>("SerialNumber")
                        .Annotation("MaxLength", 14);

                    b.HasKey("DeviceId");
                });
        }
    }
}
