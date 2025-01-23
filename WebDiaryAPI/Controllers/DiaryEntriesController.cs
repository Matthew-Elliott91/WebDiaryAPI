﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using WebDiaryAPI.Data;
using WebDiaryAPI.Models;

namespace WebDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryEntriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiaryEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries()
        {
            return await _context.DiaryEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryEntry>> GetDiaryEntry(int id)
        {
            DiaryEntry diaryEntry = await _context.DiaryEntries.FindAsync(id);

            if (diaryEntry == null)
            {
                return NotFound();
            }

            return diaryEntry;
        }

        [HttpPost]
        public async Task<ActionResult<DiaryEntry>> PostDiaryEntry(DiaryEntry diaryEntry)
        {
            diaryEntry.Id = 0;
            _context.DiaryEntries.Add(diaryEntry);
            await _context.SaveChangesAsync();
            var resourceURL = Url.Action(nameof(GetDiaryEntry), new { id = diaryEntry.Id });
            return Created(resourceURL, diaryEntry);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<DiaryEntry>> PutDiaryEntry([FromBody] DiaryEntry diaryEntry, int id)
        {
            if (id != diaryEntry.Id)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(diaryEntry).State = EntityState.Modified;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaryEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();


        }
        private bool DiaryEntryExists(int id)
        {
            return _context.DiaryEntries.Any(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DiaryEntry>> DeleteDiaryEntry(int id)
        {
            DiaryEntry diaryEntry = await _context.DiaryEntries.FindAsync(id);


            if (diaryEntry == null)
            {
                return NotFound();
            }
            else
            {
                _context.DiaryEntries.Remove(diaryEntry);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        //Another method that is synchronous


        //Another method that is a synchronous


        //Another method that is synchronous
    }
}
