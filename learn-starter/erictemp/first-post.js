import Link from "next/link"
import Head from "next/head"
import Layout from "../components/layout"

export default function FirstPost() {
  return (
    <Layout>
    <Head>
        <title>First Post</title>
    </Head>
      <h1>First Post</h1>
      <h2>
        <a href="/">
          Back to home
        </a>
      </h2>
    </Layout>
  )
}