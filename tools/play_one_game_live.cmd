@echo off
C:\Python27\python "%~dp0playgame.py" -So --engine_seed 42 --player_seed 42 --end_wait=0.25 --verbose --log_dir game_logs --turns 1000 --map_file "%~dp0maps\maze\maze_04p_01.map" %* "python ""%~dp0sample_bots\python\HunterBot.py""" "python ""%~dp0sample_bots\python\LeftyBot.py""" "python ""%~dp0sample_bots\python\HunterBot.py""" "python ""%~dp0sample_bots\python\LeftyBot.py""" | "%PROGRAMFILES(x86)%\Java\jre6\bin\java" -jar visualizer.jar

