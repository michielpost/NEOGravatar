# Neo Gravatar

Upload a personal gravatar to the NEO blockchain. The gravatar image can be shown on websites next to your NEO address. So it shows a custom picture for your address.

Inspired by gravatar.com where you can upload a gravatar for your e-mail address.

## How does it work?

The NeoGravatar.Web website is allows you to upload a picture. The website is responsible for resizing the pictue and then converts the picture to a base64 string. This makes sure all the pictures are in the same format

The base64 string can easily be stored on the blockchain. The website can submit the string to the blockchain or you can interact with the Smart Contract using neo-gui.

Using the website, you can request an image for an address and the website will retreive it from the storage of the Smart Contract on the NEO blockchain.

### MD5, SHA1 or Public Address?
This service is inspired by gravatar.com, which used MD5 to hash e-mail addresses. This is of course needed to prevent e-mail scraping and spam. So you can request an image not by it's e-mail address, but only by giving the MD5 hash of the e-mail address. This way the e-mail address can stay on the server.

I'm not sure if this will be an issue for public NEO addresses. But I wanted to try to make a similar setup. In a NEO Smart Contract, it's not possible to create an MD5 hash, however, it's easy to use the SHA1 hash, it's a static method on the SmartContract class.

## Technology Stack
- NEO Blockchain (testnet)
- C# Smart Contract
- ASP.Net Core MVC website

### External Libraries
- ImageSharp for cross platform image resizing and base64 generation
- Neo-Lux to interact with the NEO Blockchain from an ASP.Net Core website

## Smart Contract

The Smart Contract is written in C# and has two operations

- Add
This operation adds or updates a base64 image string to the storage of the smart contract. The Sha1 hash of the public address is used as storage key.
Before adding the image to the storage, the smart contract checks if the user that is calling the smartcontract is the owner of the address he wants to submit a new image for. This is done by `Runtime.CheckWitness`  
Parameters: operation name: 'add', creator address, base64 string

- Delete
This operation deletes an image from the smart contracts's storage. Only the owner of the address can remove the image.   
Parameters: operation name: 'delete', creator address


## Website

The website makes it easy to interact with the smart contract. It should be easy to upload an image without any technical skills. The website will automatically resize the image and submit it to the blockchain.

The website also hosts an endpoint which serves all the base64 images to external websites that want to integrate with this service.

## Installation

- Open the solution file in Visual Studio 2017
- Restore NuGet packages
- Compile

#### SmartContract
- NeoGravatar.Contracts.NeoGravatarContract.cs will be compiled to `src\NeoGravatar.Contracts\bin\Debug\NeoGravatar.Contracts.avm`
- Upload this to your private net or use the contract on the testnet

#### Website
- Modify the `_scriptHash` in `HomeController.cs` to your custom scripthash if you're using a private net.

## Screenshots

![Homepage](https://raw.githubusercontent.com/michielpost/NEOGravatar/master/screenshots/01_homepage.PNG)  
![Upload](https://raw.githubusercontent.com/michielpost/NEOGravatar/master/screenshots/02_upload.PNG)  
![Finished](https://raw.githubusercontent.com/michielpost/NEOGravatar/master/screenshots/03_finished.PNG)  

## Roadmap

There's still work to do to finish this project.
- Website design
- Better website to interact with the Smart Contract
- Provide documentation for developers that want to integrate NEO Gravatars on their website
- Add management functionality to the Smart Contract to be able to delete inappropriate gravatars
