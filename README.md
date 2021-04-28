# CleemyRecruitmentTest

Exemples de requêtes :
==
Toutes les dépenses :
-
GET api/Expenses



Une dépense :
-
GET api/Expenses/2



Les dépense d'un utilisateur triées par montant décroissant :
-
GET api/Expenses?userFullName=Anthony Stark&sortType=amount&sortOrder=desc



Toutes les dépense triées par date croissante :
-
GET api/Expenses?&sortType=date



Création d'une dépense :
-
POST api/Expenses

body :
    
{
    "comment": "déplacement",
    "userFullName": "Natasha Romanova",
    "currency": "USD",
    "amount": 62,
    "nature": "Hotel",
    "date": "02/03/2021"
}
