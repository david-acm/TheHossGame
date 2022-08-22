Write-Information -Message "ℹ Running pre-commit cspell check ..."

$errors = (cspell "/tests/**/*" --no-progress --unique) -Split '\n'
$errors += ((cspell "/src/**/*" --no-progress --unique) -Split '\n')

if ($errors.length -gt 0) {
    Write-Error -Message "⛔ Ups! Spell errors found.
    
    If the words are legit please add them to the project-words.json dictionary file, or add the appropiate exception to the cspell.json config file.
    
    Found words were:
    
    "
    $errors 
    | ConvertFrom-String -Delimiter ' - Unknown word ' -PropertyNames File, Word
    | Format-Table -Property Word, File

    exit 1
}