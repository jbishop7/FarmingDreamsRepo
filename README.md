# Farming Dreams
## A 2D Unity Game Developed by Jacob Bishop and Chris Chen
### rbishop7@uwo.ca 
### cchen842@uwo.ca

The main branch is farm2-jacob, don't ask...

# How to Play
First things first, you need to download the repository. You can do this by clicking the green code button and downloading as a ZIP file. 

Now that you have the repository, you have access to all the source code files, AS WELL AS the executable game. 

The executable game is located under "Executable". You will find FarmingDreams.exe inside it. Open it.

When you want to run the game, you will need to accept the potential risk. I promise there is no malicious code within this game. 

## Important Controls
- WASD to move
- Mouse for using tools
- U to view Tech Tree
- K to view your current Tech Upgrades
- I to view your inventory
- J to view your journal
- X to heal in a dungeon (if you have berry aid!)
- Esc to view the pause menu, and to dismiss any other UI. 

## Dev Tooling
- There is a case where enemies get stuck in walls in the Dungeon. If this happens, Press P to complete the dungeon. 


## Story
You are a retired Dream Warrior, finally enjoying your retirement. Unlucky for you, the World of Nightmares is getting stronger, and it's your job to tear it down. 

You have 10 days to use the land granted to you to create new weapons and items, and use them to defeat the King of Nightmares. 

## Gameplay Loop
You start on the farm. Press J to open your journal and find some starter tasks. 

I recommend starting by chopping all the trees, and fixing a bamboo plantation and the crafting bench. Craft yourself a bamboo sword!

(If you are reading the journal you should be able to complete everything without this readme)

You will pass out at 2am, and won't have the chance to prepare for the World of Nightmares. Make sure you get to bed on time to make the proper preparations!

## After the First Day
After the first day, the farm begins to open up. It's time to explore your land! Upgrade with Tech Points, and find new crops to farm. 

Maurice is a very nice fella, and wants to trade with you! You should see what he has in stock. 

## Trading with Maurice
While Maurice is a very nice fella, he is a stubborn trader. You will NOT be creating infinite money with this guy, not a chance. 
Maurice can sell you items that aren't found on the farm. You need INGOTS to create better weapons! Lucky for you, Maurice has them for a price. 

Maurice also sells buffs, and will buy anything you harvested. 

## Entering a Dungeon
You can enter the World of Nightmares Dungeons in 2 ways
1. Fall Asleep at 2am (BAD)
2. Prepare at the tent and go to sleep on time (GOOD)

Falling asleep at 2am is a bad idea. Nothing good happens after 2am! You won't be able to prepare your weapons or your buffs, and will be stuck with a woodcutters axe. You will be fodder to the foul beasts!

Preparing is a much better option. At the tent, you can select 2 weapons to bring into the World of Nightmares, as well as select buffs if you have any. Your buffs last the entire duration of the dungeon!

## Beating a Dungeon
If you beat all the enemies in the Nightmare World, you will receive an important reward - Dream Ingots. Use these to craft specialized weapons!

## Failing a Dungeon
That's okay. Not everyone is perfect. You don't get any rewards, and will wake up on the farm. 


# Features Implemented
### On the farm
- NPC Trading
- Repairing broken structures
- Programmatically save and re-build the farm scene (tracking positions and building types to ensure players come back to the farm they left!)
- Technology Points -> used to unlock new weapons, items, and farming perks!
- Crafting System -> You can craft new items and weapons.
- Breakable structures -> trees will despawn when destroyed. 
- Inventory -> Track all your tools and items.
- Journal -> Stuck? Press J! The journal will give you things to complete each day. 
- Fully animated -> All characters have full movement animations. 
- Complete UI -> Everything you would ever need to track is available on the UI. 
- Day to Night -> Farming Dreams runs on a day-night cycle. Farm by day, fight by night!
- Preparation -> Select your tools to take into the Nightmare World!

### In the Nightmare World
- Automated dungeon creation and enemy spawning
- Unique enemies
- Multiple weapon types implemented (sword, greatsword, gun, rpg)
- Healing -> Press X to regain some health (if you have berry aid!)
- Buffs -> Bring some buffs into the Dungeon to feel the effects while you fight!
