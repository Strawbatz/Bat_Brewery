VAR mood = "normal"
Hi Fabian! How are you today?
+[Good]
    ->ChosenGood
+[Great!]
    ->ChosenGreat
+[Could be better]
    ->ChoosenCouldBeBetter

===ChosenGood===
~ mood = "happy"
Good that you are feeling good today!
->END

===ChosenGreat===
~ mood = "negative"
That is amazing thay you are feeling great today!
->END

===ChoosenCouldBeBetter===
~ mood = "surprised"
Oh, sorry to hear that. I hope that the rest of your day will be better. 
-> END