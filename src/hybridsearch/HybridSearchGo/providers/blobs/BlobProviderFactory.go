package blobs

import (
	"github.com/Azure/azure-sdk-for-go/sdk/storage/azblob"
	"hybrid-search/webapi/providers"
)

type BlobProviderFactory interface {
	Create(client *azblob.Client, collectionName string) providers.BlobProvider
}
