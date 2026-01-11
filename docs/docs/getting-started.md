# Getting Started

This page contains sample BOOSE programs that demonstrate drawing, variables, arrays, conditions, loops, and methods. Each example is labelled as **restricted** (works with the default classes) or **unrestricted** (requires you to replace the corresponding class, such as Int, Real, Array, If, While, For, or Method).

## Running BOOSEapp in Visual Studio Code 

To run BOOSEapp from VS Code:

1. Open the BOOSEapp project folder in Visual Studio Code.
2. Make sure the C# extension is installed so VS Code recognises the project.
3. When VS Code prompts you to create a launch configuration for the project, accept it (or choose **Run > Run Without Debugging** once so it creates one automatically).
4. Open any C# file from the BOOSEapp project so VS Code knows which project to run.
5. Press **Ctrl+F5** or choose **Run > Run Without Debugging**.
6. When the BOOSEapp window opens, paste one of the example programs from this page into the editor area and run it there.

---

# Getting Started

This page contains sample BOOSE programs that demonstrate drawing, variables, arrays, conditions, loops, and methods. Each example is labelled as restricted or unrestricted in the comments.

```text
1. Basic drawing restricted:
*demonstrate BOOSE working with no commands that violate restrictions
moveto 100,100
pen 0,255,0
circle 50
pen 255,0,0
moveto 150,50
rect 50,100

2. basic drawing unrestricted:
*to run this program you will need to replace several drawing methods and facilities.
moveto 100,150
pen 0,0,255
circle 150
pen 255,0,0
moveto 150,50
rect 150,100
moveto 150,200
pen 0,0,255
circle 250
pen 255,0,0
moveto 200,250
rect 200,100

3. restricted int
*use of variables that won't violate restrictions
int radius = 50
int width
width = 2*radius
int height = 100
int colour = 255
pen colour,0,0
moveto 100, 100
circle radius
pen 0,colour,0
rect width, height

4. unrestricted int
*use of variables that will violate restrictions, need to replace int class to get this to run
int radius = 50
int width
int number
width = 2*radius
int height = 100
int red = 255
int green = 128
pen red,0,0
moveto 100, 100
circle radius
rect width, height
pen red,0,0
moveto 150, 150
circle radius

5. restricted reals
*demonstrate use of real values with no restriction violations
pen 0,0,255
real length = 15.5
real width = 10.0
write length * width

6. unrestricted reals
*demonstrate use of real values which violates restrictions. You will need to replace the Real class to get this working
pen 0,0,255
real length = 15.5
real width = 10.0
real pi = 3.14159
real radius = 27.7
real circ = 2 * pi * radius
real another
real more
moveto 100,100
write length * width
moveto 100,125
write circ
circle circ

7. restricted array
*int and real arrays; second half needs Array replacement
int x = 0
real y
array int nums 10
poke nums 5 = 99
peek x = nums 5
circle x
array real prices 10
poke prices 5 = 99.99
peek y = prices 5
write "£"+y
int x = 0
real y
real z
array int nums 10
poke nums 5 = 99
peek x = nums 5
circle x
array real prices 10
poke prices 5 = 99.99
peek y = prices 5
pen 0,255,0
write "£"+y
array real logs 10
poke logs 5 = 100.001
peek z = logs 5
moveto 0,25
write z

8. unrestricted array
*unrestricted array example, you will need to replace the Array class to get this to run.
int x
real y
real z
array int nums 10
array real prices 10
array real logs 10
poke nums 5 = 99
peek x = nums 5
circle x
pen 0,255,0
poke prices 5 = 99.99
peek y = prices 5
write "£"+y
array real logs 10
poke logs 5 = 100.01
peek z = log 5
moveto 0,25
write z

9. restricted ifs
*example of an if statement which won't violate restrictions
*change the value of control to see the circle change size
int control = 50
if control < 10
    pen 255,0,0
    circle 20
else
    pen 0,255,0
    circle 100
end if

10. unrestricted ifs
*example of an if statement which will violate restrictions, you will need to replace the If class.
*change the value of control to see the circle change size
int control = 50
if control < 10
    if control < 5
        pen 255,0,0
    else
        pen 0,0,255
    end if
    circle 20
    rect 20,20
else
    pen 0,255,0
    circle 100
    rect 100,100
end if

11. restricted while
*example of a while statement which won't violate restrictions
moveto 100,100
int width = 9
int height = 100
pen 255,128,0
while height > 50
    circle height
    height = height - 15
end while
pen 0,255,0
moveto 25,25
rect 100,100

12. unrestricted while
*example of a while statement which will violate restrictions, you will need to replace the While class.
moveto 100,100
int width = 9
int height = 150
pen 255,128,128
while height > 50
    circle height
    height = height - 15
    if height < 100
        pen 0,128,255
    end if
end while
pen 0,255,0
moveto 50,50
height = 50
while height > 10
    rect height, height
    height = height - 10
end while

13. restricted for
*example of a for statement which won't violate restrictions
pen 255,0,0
moveto 100,100
for count = 1 to 10 step 2
    circle count * 10
end for

14. unrestricted for
*example of a for statement which will violate restrictions, you will need to replace the For class.
pen 255,0,0
moveto 200,200
for count = 1 to 20 step 2
    circle count * 10
end for
pen 0,255,0
moveto 150,150
for count = 20 to 1 step -2
    circle count * 10
end for
pen 0,0,255
for count2 = 30 to 10 step -1
    circle count2 * 20
end for

15. restricted method
method int mulMethod int one, int two
  mulMethod = one * two
end method
method int divMethod int one, int two
  divMethod = one / two
end method
int global = 55
call mullMethod 10 5
moveto 100,100
write mullMethod
call mullMethod 10 5
moveto 100,200
write divMethod

16. unrestricted method
method int testMethod int one, int two
  testMethod = one * two
end method
int global = 55
call testMethod 10 15
moveto 200,200
write testMethod
circle testMethod
rect global, global
method int mulMethod int one, int two
  mulMethod = one * two
end method
method int divMethod int one, int two
  divMethod = one / two
end method
int global = 55
call mullMethod 10 5
moveto 100,100
write mullMethod
call divMethod 10 5
moveto 100,200
write divMethod

