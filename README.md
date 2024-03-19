# Sales
A test task for a fullstack trainee position.

# Api
## Account Controller
### Registration
Request example:
>[httppost] "api/Account/register"
>
Response:
> Success (200)
>{
  "username": "string",
  "token": "string"
}
>
>Returns a unique username and authorization JWT token
>

### login
Request example:
>[httppost] 
>https://localhost:5000/api/Account/login?login=xxxx

>Uses username also as a password
>
Response:

> Success (200)
>{
  "username": "string",
  "token": "string"
}

## Books:

### Get available Books

Returns a list of books which are available for purchase

Request example:
>[httpget] "api/Books"
>
>Anonimous requests from a certain client are allowed
>
>Response:

> Success (200)
>[
  {
    "title": "string",
    "author": "string",
    "year": 0,
    "isbn": 0,
    "image": "string",
    "price": 0,
    "available": 0
  }
]

## Order controller

### Get the current user's order
Request example:
>[HttpGet]
>
>Only authorized users are allowed to make this request
>
>/api/Order/current

Returns an order dto entity:
>{
    >
 > "id": 0, // order's id
 >
  >"books": [ // the list of books
    >
   > {
      "author": "string",
      "year": 0,
      "isbn": 0,
      "image": "string",
      "price": 0,
      "available": 0
    }
  >],
  >
  >"userName": "string",
  >
  >"createdAt": "2024-03-18T08:54:12.781Z",
  >
  >"placed": true, // if user has placed the order
  >
  >"processed": true, // if the store has processed the order
  >
  >"orderUrl": "string" // a link for the user to proceed
  >
>}

## Add a book to the current order
Request example:
>[HttpGet]
>
>Only authorized users are allowed to make this request
>
>/api/Order/addBook/{isbn} 

## Delete a book from the current order
>[HttpDelete]
>
>Only authorized users are allowed to make this request
>
>/api/Order/{isbn}  
>
>Only authorized users are allowed to make this request
>
>Returns a response with a status code

## Place order.
Gets a request from user. If the current order's sum is more or equal to 2000 the order is marked as placed.
>[HttplPost]
>
>https://localhost:5000/api/Order/sendurl?username=Y5GSSO&url=url

Response:
>Status code;

## Get a certain amount of unprocessed but placed orders
Request example:
>/getorders/{quantity}

This request is meant to be performed from a third party service.
At the developing stage unauthorized requests are allowed.

Returns a list of usernames (promo codes) and corresponding isbns.
The list contains "quantity" or less objects.

Response example:
200
>[
 >> {
    >>> "promo": "string",
    >>>
    >>>"isbns": [0]
    >>>
  >>}
  >>
>]

## Sending url for the user to proceed 

This request is meant to be performed from a third party service.
At the developing stage unauthorized requests are allowed.

Sends a url to the certain user

Request example:

>[httppost]
>
>https://localhost:5000/api/Order/sendurl?username=Y5GSSO&url=url

## Cancel an order

This request is meant to be performed from a third party service.
At the developing stage unauthorized requests are allowed.

Request example:

>[httppost]
>https://localhost:5000/api/Order/cancelorder?username=xxxx

Returns action result.

## Delete order

This request is meant to be performed from a third party service.
At the developing stage unauthorized requests are allowed.

Deletes a completed order.

>https://localhost:5000/api/Order/cancelorder?username=xxxx

Returns action result


