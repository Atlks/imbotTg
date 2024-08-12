package lib

import (
	"bytes"
	"crypto/aes"
	"crypto/cipher"
	"crypto/rand"
	"encoding/hex"
	"fmt"
	"io"
	"strings"
)

// Encrypt AES encryption function
func Encrypt(plaintext string, key string) (string, error) {
	// Convert key to byte slice
	keyBytes := []byte(key)

	// Create a new AES cipher block
	block, err := aes.NewCipher(keyBytes)
	if err != nil {
		return "", err
	}

	// Convert plaintext to byte slice
	plainTextBytes := []byte(plaintext)

	// Generate a random initialization vector (IV)
	iv := make([]byte, aes.BlockSize)
	if _, err := io.ReadFull(rand.Reader, iv); err != nil {
		return "", err
	}

	// Create a new AES cipher in CBC mode
	mode := cipher.NewCBCEncrypter(block, iv)

	// Pad plaintext to be a multiple of the block size
	padding := aes.BlockSize - len(plainTextBytes)%aes.BlockSize
	padText := bytes.Repeat([]byte{byte(padding)}, padding)
	paddedPlaintext := append(plainTextBytes, padText...)

	// Encrypt plaintext
	encrypted := make([]byte, len(paddedPlaintext))
	mode.CryptBlocks(encrypted, paddedPlaintext)

	// Return the IV and encrypted text as a single hex string
	return hex.EncodeToString(iv) + ":" + hex.EncodeToString(encrypted), nil
}

// Decrypt AES decryption function
func Decrypt(ciphertext string, key string) (string, error) {
	// Convert key to byte slice
	keyBytes := []byte(key)

	// Split the IV and encrypted text
	parts := strings.Split(ciphertext, ":")
	if len(parts) != 2 {
		return "", fmt.Errorf("invalid ciphertext format")
	}

	iv, err := hex.DecodeString(parts[0])
	if err != nil {
		return "", err
	}

	encrypted, err := hex.DecodeString(parts[1])
	if err != nil {
		return "", err
	}

	// Create a new AES cipher block
	block, err := aes.NewCipher(keyBytes)
	if err != nil {
		return "", err
	}

	// Create a new AES cipher in CBC mode
	mode := cipher.NewCBCDecrypter(block, iv)

	// Decrypt the ciphertext
	decrypted := make([]byte, len(encrypted))
	mode.CryptBlocks(decrypted, encrypted)

	// Remove padding
	padding := int(decrypted[len(decrypted)-1])
	if padding > aes.BlockSize || padding > len(decrypted) {
		return "", fmt.Errorf("invalid padding")
	}
	decrypted = decrypted[:len(decrypted)-padding]

	return string(decrypted), nil
}

func main() {
	key := "examplekey123456" // AES key must be 16, 24, or 32 bytes long
	plaintext := "dddd!"

	// Encrypt
	encrypted, err := Encrypt(plaintext, key)
	if err != nil {
		fmt.Println("Encryption error:", err)
		return
	}
	fmt.Println("Encrypted:", encrypted)

	// Decrypt
	decrypted, err := Decrypt(encrypted, key)
	if err != nil {
		fmt.Println("Decryption error:", err)
		return
	}
	fmt.Println("Decrypted:", decrypted)
}
