Write-Information -Message "ℹ Running pre-commit cspell check ..."

$branch = &git rev-parse --abbrev-ref HEAD 

if ($branch -match '^(feature|bug|hotfix)\/([0-9]+)-([A-z-])+$') {
    exit 0
}

Write-Error -Message "⛔ Ups! Branch name error found.
    
    The branch name you are trying to commit to doesn't follow the standard:
    
    type/<workitem-number>-<short-description> verified with the regex:

    ^(feature|bug|hotfix)\/([0-9]+)-([A-z-])+$
    
    Please create a branch tha follows the standard and try again
    
    "
exit 1