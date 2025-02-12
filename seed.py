import json
import psycopg2
import random

# Connect to PostgreSQL
conn = psycopg2.connect(
    dbname="mydb",
    user="postgres",
    password="1234",
    host="localhost",
    port="5432"
)
cursor = conn.cursor()

# Load JSON file
with open("Users.json", "r") as file:
    users = json.load(file)

with open("Roles.json", "r") as file:
    roles = json.load(file)

with open("Address.json", "r") as file:
    addresses = json.load(file)

random_user_id = 0
# # Insert data into PostgreSQL
# for user in users:
#     i+=1 
#     roleId = random.randint(1,4)
#     cursor.execute(
#         """
# INSERT INTO public."Users"(
# 	"Firstname", "Surname", "Gender", "RoleId")
# 	VALUES (%s, %s, %s, %s);
#         """,
#         (user["FirstName"], user["LastName"], user["Gender"], roleId)
#     )

userId = 1
for user in users:

    randAddress = random.randint(0, 9)

    ii = 1
    while ii < 4:
        randAddress = random.randint(0, 9)

        cursor.execute(
            """
            INSERT INTO public."Addresses"(
        "Street"
    , "City", "State", "ZipCode", "UserId")
        VALUES ( %s, %s, %s, %s, %s);
    """,
            (addresses[randAddress]["Street"], addresses[randAddress]["City"], addresses[randAddress]["State"], addresses[randAddress]["Zip"], userId)
        )
        ii += 1
    
    userId+=1
 

# Commit and close connection
conn.commit()
cursor.close()
conn.close()
