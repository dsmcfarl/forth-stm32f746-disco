0 variable line-x1   0 variable line-y1
0 variable line-sx   0 variable line-sy
0 variable line-dx   0 variable line-dy
0 variable line-err


0 variable ellipse-xm   0 variable ellipse-ym
0 variable ellipse-dx   0 variable ellipse-dy
0 variable ellipse-a    0 variable ellipse-b
0 variable ellipse-a^2  0 variable ellipse-b^2
0 variable ellipse-err

: ellipse-putpixel ( y x -- ) ellipse-xm @ + swap ellipse-ym @ + putpixel ;

: ellipse-step ( -- )
    ellipse-dy @        ellipse-dx @        ellipse-putpixel
    ellipse-dy @ negate ellipse-dx @        ellipse-putpixel
    ellipse-dy @ negate ellipse-dx @ negate ellipse-putpixel
    ellipse-dy @        ellipse-dx @ negate ellipse-putpixel

    ellipse-err @ 2* >r
    r@  ellipse-dx @ 2* 1+ ellipse-b^2 @ *        < if  1 ellipse-dx +! ellipse-dx @ 2* 1+ ellipse-b^2 @ *        ellipse-err +! then
    r>  ellipse-dy @ 2* 1- ellipse-a^2 @ * negate > if -1 ellipse-dy +! ellipse-dy @ 2* 1- ellipse-a^2 @ * negate ellipse-err +! then
;


