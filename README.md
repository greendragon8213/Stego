## About The Project

**SUDOSTEGO** is a steganography system.

<ins>**Steganography**</ins> is the technique of hiding secret data within ordinary, non-secret data. Despite cryptography, it is aimed not only to encrypt but also to avoid detection - to hide the fact that there is secret data at all.

### Definitions (domain glossary)

- <ins>**Secret**</ins> - payload: the information to be concealed;
- <ins>**Container**</ins> - object where secret data can be embedded;
- <ins>**Key (stegokey)**</ins> - key for encryption, decryption and detection of secret data;
- <ins>**Stegocontainer (carrier)**</ins> - object with embedded secret data in it; result of encryption process;
- Actions:
  - <ins>**Encryption (embedding)**</ins> - process of hiding secret data into the container using key;
  - <ins>**Decryption (extraction)**</ins> - process of extracting concealed secret from stegocontainer using key.

### Features of SUDOSTEGO

- Allows to hide any types of files into images (you can specify as container any image type, but stegocontainer will be created as a bmp file);
- Uses ordinary password as a key;
- Stegosystem capacity (the ratio between the container and the secret data) is 1/2; to be precise, 2 pixels of container can store 3 bytes of secret;
- Works offline (locally) - there is no data transfer through the network - hence, there are no risks of data leaks;

## Usage

To install the application, click [here](https://greendragon8213.github.io/Stego/installation/Installer/setup.exe).

Application consists of two tabs: encryption (1) and decryption (2).

1. **Encryption tab** - embedding secret file: <br><br>
Launch the app, and Encryption tab will show up:
![Encryption tab - 1](https://github.com/greendragon8213/Stego/blob/demo-files/DemoScreens/1-encr.png)<br><br>
Provide container image, secret file, output path and password:
![Encryption tab - 2](https://github.com/greendragon8213/Stego/blob/demo-files/DemoScreens/2-encr.png)<br><br>
Click "encrypt" - stegocontainer (image with an embedded secret file) will be created, and a notification will appear in the bottom:<br>
![Encryption tab - 3](https://github.com/greendragon8213/Stego/blob/demo-files/DemoScreens/3-encr.png) <br><br>
A created bmp image (stegocontainer) contains a secret file, which can be extracted or detected only through decryption with the correct password. Now, the secret can be securely sent or stored within the stegocontainer.<br><br>
2. **Decryption tab** - extracting secret file:<br><br>
Open the Decryption tab and provide the stegocontainer path, output path, password; click "decrypt" - secret file will be extracted and notification will appear in the bottom: <br>
![Decryption tab - 4](https://github.com/greendragon8213/Stego/blob/demo-files/DemoScreens/4-decr.png)

## Built With

- .NET Framework 4.7
- WPF
- MahApps.Metro
- MVVM Light Toolkit
