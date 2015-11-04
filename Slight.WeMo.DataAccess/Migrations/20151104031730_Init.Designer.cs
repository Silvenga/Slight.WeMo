using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Slight.WeMo.DataAccess;

namespace Slight.WeMo.DataAccess.Migrations
{
    [DbContext(typeof(WeMoContext))]
    [Migration("20151104031730_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
        }
    }
}
