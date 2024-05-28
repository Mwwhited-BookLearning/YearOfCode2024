package lexical

import (
	"fmt"
)

type OpenSearchOptions struct {
	HostName  string
	Port      int
	IndexName string

	UserName string
	Password string
}

func CreateOptions() OpenSearchOptions {
	return OpenSearchOptions{
		HostName:  "192.168.1.170",
		Port:      9200,
		IndexName: "docs",
	}
}

func (opt OpenSearchOptions) Url() string {
	return fmt.Sprintf("http://%s:%v", opt.HostName, opt.Port)
}
