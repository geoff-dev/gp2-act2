## **Game Programming 2 - Activity 2**

Entity script is the base class of the three main types divided into DPS Entity, Tank Entity, and Support Entity.

Each Entity shares the base stats such as health, experience, navigation, damage and skills.

In order to utilize the Entity class, the children class differs from each other:
1. DPSEntity class is responsible for having high damage. It has dash as navigate skill. Upon dying, DPS Entity will do a last chance to deal more damage for short duration.
2. TankEntity class is responsible for absorbing damage. It has a crowd control as navigate skill. Upon dying, Tank Entity will explode.
3. SupportEntity class is responsible for giving utility to team. It has boost movement as navigate skill. Upon dying, Support Entity will place a curse on nearby enemies.