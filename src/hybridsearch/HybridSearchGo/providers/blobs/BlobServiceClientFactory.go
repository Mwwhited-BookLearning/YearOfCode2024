package blobs

import (
	"github.com/Azure/azure-sdk-for-go/sdk/storage/azblob"
)

type BlobServiceClientFactory interface {
	Create(options AzureBlobProviderOptions) *azblob.Client
}
