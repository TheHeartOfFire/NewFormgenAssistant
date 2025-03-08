using AMFormsCST.Core.Interfaces;
using AMFormsCST.Core.Types;

namespace AMFormsCST.Core;
public class Notebook : INotebook
{
    public IList<Note> Notes { get; private set; } = [new Note()];
    public Note CurrentNote { get; private set; }

    public Notebook()
    {
        CurrentNote = Notes[0];
    }

    public void AddNote(bool select = false) => AddNote(new Note(), select);
    public void AddNote(Note note, bool select = false)
    {
        Notes.Add(note);

        if (select)
            CurrentNote = note;

    }

    public void RemoveNote(Note note)
    {
        if (!Notes.Contains(note))
            throw new NullReferenceException(
                "There is no Note in Notes that matches the note provided.",
                new NullReferenceException($"note is missing:\n{note.Dump()}"));
        Notes.Remove(note);

        if (Notes.Count == 0)
        {
            CurrentNote = new Note();
            Notes.Add(CurrentNote);
            return;
        }

        if (CurrentNote == note)
            CurrentNote = Notes[0];
    }

    public void Clear()
    {
        Notes.Clear();
        CurrentNote = new Note();
        Notes.Add(CurrentNote);
    }

    public void SelectNote(Note note)
    {
        if (!Notes.Contains(note))
            throw new NullReferenceException(
                "There is no Note in Notes that matches the note provided.",
                new NullReferenceException($"note is missing:\n{note.Dump()}"));
        CurrentNote = note;
    }

    public void SwapNotes(Note note1, Note note2)
    {
        if (!Notes.Contains(note1) || !Notes.Contains(note2))
        {
            var offender = !Notes.Contains(note1) ? note1 : note2;
            bool both = !Notes.Contains(note1) && !Notes.Contains(note2);

            throw new NullReferenceException(
                "There is no Note in Notes that matches the note provided.",
                new NullReferenceException(
                    both ? "Both notes are missing:\n" + note1.Dump() + '\n' + note2.Dump() :
                    offender == note1 ? "note1 is missing:\n" + note1.Dump() :
                    "note2 is missing:\n" + note2.Dump()));
        }

        var index1 = Notes.IndexOf(note1);
        var index2 = Notes.IndexOf(note2);
        Notes[index1] = note2;
        Notes[index2] = note1;
    }

    internal void Load(List<Note> notes)
    {
        if (notes.Count == 0)
            throw new NullReferenceException("There are no notes to load.");

        Notes = notes;
        CurrentNote = Notes[0];
    }
}
