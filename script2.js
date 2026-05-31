function setLanguage(lang)
{
    
    if(lang === 'en')
    {
        document.getElementById('title').innerText = 'Labyrinth Game Documentation';
        document.getElementById('subtitle').innerText = 'Console C# project presentation';

        document.getElementById('introTitle').innerText = 'Project Goal';
        document.getElementById('introText').innerText = 'The goal of the game is to discover every treasure room and escape through an exit. The game runs in the console and the player moves with the WASD keys.';
        document.getElementById('introTextCopy').innerText = 'Explore the rooms, remember the routes, then find the exit. This documentation briefly presents the gameplay, controls, and implemented methods.';

        document.getElementById('teamTitle').innerText = 'Project Work';
        document.getElementById('teamText').innerText = 'The project was completed by 3 members with the following responsibilities:';

        document.getElementById('member1').innerText = 'Creating the labyrinth game : Sándor Benedek Tóth';
        document.getElementById('member2').innerText = 'Creating the map editor : Szabolcs Oláh';
        document.getElementById('member3').innerText = 'Implementing the required methods and building the website: Tamas Jakab';

        document.getElementById('footerText').innerText = 'Created by: Tamas Jakab';
        document.getElementById('controlsTitle').innerText = 'Controls';

        document.getElementById('northText').innerText = 'Move north';
        document.getElementById('westText').innerText = 'Move west';
        document.getElementById('southText').innerText = 'Move south';
        document.getElementById('eastText').innerText = 'Move east';

        document.getElementById('mapTitle').innerText = 'Map Characters';
        document.getElementById('charText').innerText = 'Character';
        document.getElementById('meaningText').innerText = 'Meaning';

        document.getElementById('method1').innerText = 'Count rooms';
        document.getElementById('method2').innerText = 'Find exits';
        document.getElementById('method3').innerText = 'Check invalid characters';
        document.getElementById('method41').innerText = 'Find unreachable paths';
        document.getElementById('method5').innerText = 'Generate labyrinth';

        document.getElementById('wallText').innerText = 'Wall / filler';
        document.getElementById('roomText').innerText = 'Treasure room';
        document.getElementById('pathText').innerText = 'Paths';

        document.getElementById('saveTitle').innerText = 'Save and Load';
        document.getElementById('saveText').innerText = 'The game supports saving and loading. Save files use the .SAV extension.';

        document.getElementById('tipsTitle').innerText = 'Tips';

        document.getElementById('tip1').innerText = 'Always check the available directions.';
        document.getElementById('tip2').innerText = 'Try to find every treasure room first.';
        document.getElementById('tip3').innerText = 'Memorize routes when using hidden map mode.';

        document.getElementById('methodsTitle').innerText = 'Implemented Methods';

    }
    else
    {
        location.reload();
    }
}
