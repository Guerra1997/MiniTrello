using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniTrello.Models;

namespace MiniTrello.Data;

public class MiniTrelloDbContext : IdentityDbContext<IdentityUser>
{
    public MiniTrelloDbContext(DbContextOptions<MiniTrelloDbContext> options)
        : base(options) { }

    public DbSet<Board> Boards => Set<Board>();
    public DbSet<BoardList> BoardLists => Set<BoardList>();
    public DbSet<Card> Cards => Set<Card>();

}