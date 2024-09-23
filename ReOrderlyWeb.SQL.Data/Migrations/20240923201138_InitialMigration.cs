using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReOrderlyWeb.SQL.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    orderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProduct = table.Column<int>(type: "int", nullable: false),
                    idOrder = table.Column<int>(type: "int", nullable: false),
                    orderItemQuantity = table.Column<int>(type: "int", nullable: false),
                    orderPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.orderItemId);
                });

            migrationBuilder.CreateTable(
                name: "OrderSubscription",
                columns: table => new
                {
                    orderSubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUser = table.Column<int>(type: "int", nullable: false),
                    idProduct = table.Column<int>(type: "int", nullable: false),
                    intervalDays = table.Column<int>(type: "int", nullable: false),
                    orderDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSubscription", x => x.orderSubscriptionId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUser = table.Column<int>(type: "int", nullable: false),
                    orderStatus = table.Column<int>(type: "int", nullable: false),
                    orderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OrderItemsorderItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.orderId);
                    table.ForeignKey(
                        name: "FK_Order_OrderItems_OrderItemsorderItemId",
                        column: x => x.OrderItemsorderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "orderItemId");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    productPrice = table.Column<double>(type: "float", nullable: false),
                    productQuantity = table.Column<int>(type: "int", nullable: false),
                    OrderItemsorderItemId = table.Column<int>(type: "int", nullable: true),
                    orderSubscriptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productId);
                    table.ForeignKey(
                        name: "FK_Products_OrderItems_OrderItemsorderItemId",
                        column: x => x.OrderItemsorderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "orderItemId");
                    table.ForeignKey(
                        name: "FK_Products_OrderSubscription_orderSubscriptionId",
                        column: x => x.orderSubscriptionId,
                        principalTable: "OrderSubscription",
                        principalColumn: "orderSubscriptionId");
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    orderStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderStatusDescription = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    orderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.orderStatusId);
                    table.ForeignKey(
                        name: "FK_OrderStatus_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "orderId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    streetName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    houseNumber = table.Column<int>(type: "int", nullable: true),
                    voivodeship = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    country = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    zipcode = table.Column<int>(type: "int", nullable: true),
                    emailAddress = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    password = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    phoneNumber = table.Column<int>(type: "int", nullable: false),
                    orderId = table.Column<int>(type: "int", nullable: true),
                    orderSubscriptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                    table.ForeignKey(
                        name: "FK_User_OrderSubscription_orderSubscriptionId",
                        column: x => x.orderSubscriptionId,
                        principalTable: "OrderSubscription",
                        principalColumn: "orderSubscriptionId");
                    table.ForeignKey(
                        name: "FK_User_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "orderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderItemsorderItemId",
                table: "Order",
                column: "OrderItemsorderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_orderId",
                table: "OrderStatus",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderItemsorderItemId",
                table: "Products",
                column: "OrderItemsorderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_orderSubscriptionId",
                table: "Products",
                column: "orderSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_orderId",
                table: "User",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_User_orderSubscriptionId",
                table: "User",
                column: "orderSubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "OrderSubscription");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "OrderItems");
        }
    }
}
