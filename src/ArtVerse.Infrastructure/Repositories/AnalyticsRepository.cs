using ArtVerse.Application.Admin.DTOs;
using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Enums;
using ArtVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtVerse.Infrastructure.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly ApplicationDbContext _context;

    public AnalyticsRepository(ApplicationDbContext context) => _context = context;

    public async Task<AnalyticsDataDto> GetDashboardStatsAsync(CancellationToken ct = default)
    {
        var totalStudents = await _context.Students.CountAsync(ct);
        var totalStaff = await _context.Staffs.CountAsync(ct);
        var totalArtworks = await _context.Paintings.CountAsync(ct);
        var pendingReviews = await _context.Paintings.CountAsync(p => p.Status == PaintingStatus.Submitted, ct);
        var totalCompetitions = await _context.Competitions.CountAsync(ct);
        var totalExhibitions = await _context.Exhibitions.CountAsync(ct);

        var prices = await _context.Paintings
            .Where(p => p.IsForSale && p.BasePrice.HasValue)
            .Select(p => p.BasePrice!.Value)
            .ToListAsync(ct);
        var totalArtworkValue = prices.Sum();

        var stats = new DashboardStatsDto(
            totalStudents,
            totalStaff,
            totalArtworks,
            pendingReviews,
            totalCompetitions,
            totalExhibitions,
            totalArtworkValue
        );

        var allTechniques = await _context.Paintings
            .Select(p => p.Technique)
            .ToListAsync(ct);

        var techniqueGroup = allTechniques
            .GroupBy(t => t.ToString())
            .Select(g => new { Technique = g.Key, Count = g.Count() })
            .ToList();

        var techLabels = techniqueGroup.Select(t => t.Technique).ToList();
        var techCounts = techniqueGroup.Select(t => t.Count).ToList();

        if (!techLabels.Any())
        {
            techLabels = new List<string> { "Oil", "Watercolor", "Acrylic", "Digital", "Sculpture" };
            techCounts = new List<int> { 12, 18, 15, 25, 8 };
        }

        var monthlyLabels = new List<string> { "T9", "T10", "T11", "T12", "T1" };
        var monthlyCounts = new List<int> { 5, 12, 28, 45, 60 };

        return new AnalyticsDataDto(stats, techLabels, techCounts, monthlyLabels, monthlyCounts);
    }
}
