package blobs

import (
	"github.com/Azure/azure-sdk-for-go/sdk/storage/azblob"
)

type BlobProviderFactory interface {
	Create(client *azblob.Client, collectionName string) BlobProvider
}
