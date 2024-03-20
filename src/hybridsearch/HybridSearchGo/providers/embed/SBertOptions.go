package embed

type SBertOptions struct {
	Url string
}

func CreateOptions() SBertOptions {
	return SBertOptions{
		Url: "http://192.168.1.170:5080",
	}
}
